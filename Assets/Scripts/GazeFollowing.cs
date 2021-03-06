﻿using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour
{

    public bool overwrite;
    GameObject Gaze, Clone, threshold, target;
   // GameObject Agent;
    float angle, maintain = -7;
    int visible = 0, prev_visible = 0;
    public float stepRadians = 0.1f * Mathf.PI / 180;
    // int counter;

    public float m = 0.66f, k = 1.38f, t = 7.0f, transferThreshold = 7;


    public enum State { Stroll, Decide, Follow, Invisible, Immune }

    State _state = State.Stroll;
    State _prevState;

    #region Basic Getters/Setters
    public State CurrentState
    {
        get { return _state; }
    }

    public State PrevState
    {
        get { return _prevState; }
    }
    #endregion

    public void SetState(State newState)
    {
        _prevState = _state;
        _state = newState;
        //   Debug.Log(_state); 
        // working correctly atm so commented out
    }


    // Use this for initialization
    void Start()
    {

        SetState(State.Stroll);

       // Agent = gameObject; //the object being called

        //    stepRadians = 0.1f* Mathf.PI / 180; //defined upstairs

        //   defaultSpd=500;
        //   maxSpeed = defaultSpd;
        //    maxSpeed = Mathf.Pow(0.02f,2)*Time.deltaTime;

        Gaze = GameObject.FindGameObjectWithTag("Gaze");

        Gaze.transform.parent.tag = "Gazing";

        // transferThreshold = 7;

        //MOVED TO IDELRUNJUMP (so as not to be executed 50 times)
        //    threshold = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //   threshold.GetComponent<Collider>().enabled = false;
        //   threshold.transform.position = Gaze.transform.parent.transform.position - new Vector3(0,Gaze.transform.parent.transform.position.y, 0);
        //  threshold.transform.localScale = new Vector3(2*transferThreshold, 0.1f, 2*transferThreshold);

        //  Clone = (GameObject) Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0));
    }

    // Update is called once per frame
    void Update()
    {


        StartCoroutine(States(gameObject)); //determine state of the agent and take appropriate actions
                                            // StartCoroutine(DirectionStates(gameObject));       // counter = 0;

        //   foreach (UnityEngine.GameObject x in UnityEngine.GameObject.FindGameObjectsWithTag("Gazing"))
        //      counter = counter++;
    }


    IEnumerator States(GameObject Agent)
    {
        // SetState(_state);

        while (true)
        {
            switch (_state)
            {
                case State.Invisible:
                    //Agent.transform.forward = Agent.transform.parent.transform.forward; 
                    //no need, no one can see :)
                    if (Agent.tag != "Agent")
                    {
                        Agent.tag = "Agent";
                    }
                     
                    if ((_prevState == State.Follow) || (_prevState == State.Decide))  //making sure it's turned off (and only checking once)
                        foreach (Transform child in Agent.transform)
                        {
                            child.gameObject.SetActive(false);

                          //  Debug.Log("Invisible");
                            SetState(_state); //regoes into state, but de-stores the previous if State.follow / decide
                        }

                    if (Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //only for visible agents
                        SetState(State.Stroll);

                    break;
                case State.Stroll:
                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    lookFwd(Agent); //normal behaviour
                    if (Agent.tag != "Agent")
                    {
                        Agent.tag = "Agent";
                    }

                    if ( (_prevState == State.Follow) || (_prevState == State.Decide) )  //making sure it's turned off (and only checking once)
                        foreach (Transform child in Agent.transform)
                        {
                            child.gameObject.SetActive(false);
                            SetState(_state); //regoes into state, but de-stores the previous if State.follow / decide
                        }

                    if (scanIncrease(Agent)) SetState(State.Decide); //if more gazers around, redecide whether to follow gaze or not

                    break;

                case State.Decide:

                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    foreach (Transform child in Agent.transform) //show when "scanning"
                    {
                        child.gameObject.SetActive(true);
                    } 

                    lookFwd(Agent); //normal behaviour
                    if (Agent.tag != "Agent")
                    {
                        Agent.tag = "Agent";
                    }

                    if (decideFollow(Agent)) SetState(State.Follow);
                    else SetState(State.Stroll);

                    break;

                case State.Follow:

                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    if (Agent.tag != "Gazing") Agent.tag = "Gazing";
                    {
                        gazeFollow(Agent); //gazing behaviour
                        
                        foreach (Transform child in Agent.transform)
                        {
                            child.gameObject.SetActive(true);
                        } 
                    }

                    if (!maintainBhv(Agent)) { //Debug.Log("Immune"); 
                        SetState(State.Immune); }

                    break;
                case State.Immune:
                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    lookFwd(Agent); //normal behaviour
                    if (Agent.tag != "Agent")
                    {
                        Agent.tag = "Agent";

                        foreach (Transform child in Agent.transform)
                        {
                            child.gameObject.SetActive(false);
                        }
                    }

                    maintain = -7; //restarting, in case gazefollow maintain bhv still ongoing but out of range
                    if (!maintainBhv(Agent))
                    {
                        Debug.Log("Not Immune"); SetState(State.Stroll);
                    }

                    break;

            }
            yield return null;
        }

    }

    bool scanIncrease(GameObject Agent)
    {
        prev_visible = visible; //prev number of visible gazers
        visible = (int)scanFrontalArea(Agent).x;//  scan for visible gazers

        if (visible > prev_visible) //if more gazers around
            return true;
        else return false;
    }

    bool decideFollow(GameObject Agent)
    {
        visible = (int) scanFrontalArea(Agent).x;

        //formula to determine whether to follow or not 
        //right now - placeholder to 2 gazers (player and static agent)

      //  if (scanFrontalArea(Agent).y > 0) Debug.Log("haoleu");

        //if (visible >= 2) return true;
        if (Random.Range(0f, 1f) < (0.6f + 0.4f*scanFrontalArea(Agent).z/visible ) * m * Mathf.Pow(visible, k) / (Mathf.Pow(visible, k) + Mathf.Pow(t, k))) return true;
        else return false;
        //function in Gallup working TOO WELL
        //function that tries to mimic findings in Gallup a - scaling via sameDir & incoming balance
        //ALTHOUGH in large situations it doesn't make that much sense
        //as same direction gazers increase, the probability of looking is higher

        //if (visible > 15)
        //  {
        //      if (Random.Range(0f, 1f) > m) return true;
        //     else return false;
        //  }
        //  else if (Random.Range(0f, 1f) < m) return true;
        //  else return false;
    }

    void lookFwd(GameObject Agent)
    {
        Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, Agent.transform.parent.transform.forward, stepRadians, 0);
        //moving gaze forward
    }

    public void gazeFollow(GameObject Agent)
    {
        Vector3 desired = Gaze.transform.position - Agent.transform.position;

        //  if (Vector3.Angle(Agent.transform.forward, desired) < 80) // check if within visual field
        //OBSOLETE - now visual field is being calculated via FSM
        //   {
        if (Vector3.Angle(Agent.transform.parent.transform.forward, desired) <= 135)
        //physically harder to look back over 135 degrees - so if going over, look away
        {
            Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, desired, stepRadians, 0);

        }
        //rotate towards target
        //steering behaviour -> normalized by stepRadians
        else Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, Agent.transform.parent.transform.forward, stepRadians, 0);
        //if not within physically comfortable angle, rotate back towards run direction
        //  }

    }

    bool maintainBhv(GameObject Agent)
    {
        if (maintain == -7)
        {
            maintain = 5000;
            //   Debug.Log(maintain);
            return true;
        } //we want follow to be maintained for 5 seconds 

        else if (maintain > 0)
        {
            maintain--;
            // Debug.Log(maintain);
            return true;
        } //counting down

        else
        {
            maintain = -7;
            //   Debug.Log(maintain);
            return false;
        } //default unmaintain value

    }

    Vector3 scanFrontalArea(GameObject Agent)
    {
        int counter = 0, incoming = 0, sameDir = 0; // upcount = 0;
        foreach (GameObject Gazing in GameObject.FindGameObjectsWithTag("Gazing")) //scanning for gazers
        {
            if (Vector3.Distance(Agent.transform.position, Gazing.transform.position) <= transferThreshold) //if any gazer within range
                if (Vector3.Angle(Agent.transform.forward, Gazing.transform.position - Agent.transform.position) <= 80) //within visual area
                    counter++;

            if (Gazing.GetComponentInParent<RandomCharacters>() == null)
            {
                if (Agent.GetComponentInParent<RandomCharacters>()._prevState == RandomCharacters.State.downwards)
                    incoming++; //as stationary gazers are looking upstream, although they don't have a Direction State 
                //due to RandomCharacters.cs only being for mobile agents
                else if (Agent.GetComponentInParent<RandomCharacters>()._prevState == RandomCharacters.State.upwards)
                    sameDir++;
            }
            else
            {
                if (Gazing.GetComponentInParent<RandomCharacters>()._prevState == Agent.GetComponentInParent<RandomCharacters>()._prevState)
                    sameDir++;
                else incoming++;
            }
            //if (Gazing.transform.parent.transform.parent.transform.parent.tag == "upwards")
            //  upcount++;
        }

        //   if (Agent.transform.parent.transform.parent.transform.parent.tag == "upwards")
        //   {  sameDir = upcount;
        //     incoming = counter - upcount;//same tag => same direction
        //}
        //  else if (Agent.transform.parent.transform.parent.transform.parent.tag == "downwards")
        // {
        //     incoming = upcount;
        //    sameDir = counter - upcount;
        // }//diff tag => incoming
        // Debug.Log(incoming);

        incoming = counter - sameDir; //juust to check

        return new Vector3 (counter, incoming, sameDir);
        //returns amount of gazing agents in one's visual area
    }
}


   
