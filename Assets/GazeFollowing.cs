using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour {

    public bool overwrite;
    GameObject Gaze, Agent, Clone, threshold, target;
    float angle, transferThreshold, stepRadians = 0.1f * Mathf.PI / 180, randomVar = 10, maintain = -7;
    int visible = 0, prev_visible = 0;

    public enum State { Stroll, Decide, Follow, Invisible }

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
    void Start () {

        SetState(State.Stroll);

        Agent = gameObject; //the object being called

    //    stepRadians = 0.1f* Mathf.PI / 180; //defined upstairs

     //   defaultSpd=500;
     //   maxSpeed = defaultSpd;
     //    maxSpeed = Mathf.Pow(0.02f,2)*Time.deltaTime;

        Gaze = GameObject.FindGameObjectWithTag("Gaze");

        Gaze.transform.parent.tag = "Gazing";

        transferThreshold = 7;

        //MOVED TO IDELRUNJUMP (so as not to be executed 50 times)
    //    threshold = GameObject.CreatePrimitive(PrimitiveType.Sphere);
     //   threshold.GetComponent<Collider>().enabled = false;
     //   threshold.transform.position = Gaze.transform.parent.transform.position - new Vector3(0,Gaze.transform.parent.transform.position.y, 0);
      //  threshold.transform.localScale = new Vector3(2*transferThreshold, 0.1f, 2*transferThreshold);

        //  Clone = (GameObject) Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0));
    }
	
	// Update is called once per frame
	void Update () {

        
            StartCoroutine(States(gameObject)); //determine state of the agent and take appropriate actions

    }

    IEnumerator States (GameObject Agent)
    {
       // SetState(_state);

        while (true)
        {
            switch (_state)
            {
                case State.Invisible:
                    //Agent.transform.forward = Agent.transform.parent.transform.forward; 
                    //no need, no one can see :)

                    if (Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //only for visible agents
                        SetState(State.Stroll);

                    break;
                case State.Stroll:
                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    lookFwd(Agent); //normal behaviour

                    if ( scanIncrease(Agent) ) SetState(State.Decide); //if more gazers around, redecide whether to follow gaze or not
                    
                    break;

                case State.Decide:

                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    lookFwd(Agent); //normal behaviour

                    if ( decideFollow(Agent) ) SetState(State.Follow);
                    else SetState(State.Stroll);

                    break;

                case State.Follow:

                    if (!Agent.transform.parent.transform.parent.transform.parent.GetComponentInChildren<Renderer>().isVisible) //not visible agents
                        SetState(State.Invisible);

                    gazeFollow(Agent); //gazing behaviour

                    if (!maintainFollow(Agent) ) SetState(State.Stroll);

                    break;
               
            }
            yield return null;
        }

    }

    bool scanIncrease(GameObject Agent)
    {
        prev_visible = visible; //prev number of visible gazers
        visible = scanFrontalArea(Agent);//  scan for visible gazers

        if (visible > prev_visible) //if more gazers around
            return true;
        else return false;  
    }

    bool decideFollow(GameObject Agent)
    { 
        visible = scanFrontalArea(Agent);

        //formula to determine whether to follow or not 
        //right now - placeholder to 2 gazers (player and static agent)

        if (visible >= 2) return true;
        else return false;
    }

    void lookFwd(GameObject Agent)
    {
        Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, Agent.transform.parent.transform.forward, stepRadians, 0);
        //moving gaze forward
    }

    void gazeFollow(GameObject Agent)
    {
        Vector3 desired = Gaze.transform.position - Agent.transform.position;

      //  if (Vector3.Angle(Agent.transform.forward, desired) < 80) // check if within visual field
      //OBSOLETE - now visual field is being calculated via FSM
     //   {
            if (Vector3.Angle(Agent.transform.parent.transform.forward, desired) <= 135)
                //physically harder to look back over 135 degrees - so if going over, look away
               Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, desired, stepRadians, 0);
            //rotate towards target
            //steering behaviour -> normalized by stepRadians
            else Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, Agent.transform.parent.transform.forward, stepRadians, 0);
            //if not within physically comfortable angle, rotate back towards run direction
      //  }

    }

    bool maintainFollow(GameObject Agent)
    {
        if (maintain == -7)
           {  maintain = 5000;
         //   Debug.Log(maintain);
            return true;
            } //we want follow to be maintained for 5 seconds 

        else if (maintain>0)
           {  maintain--;
           // Debug.Log(maintain);
            return true;
            } //counting down

        else
           {  maintain = -7;
         //   Debug.Log(maintain);
            return false;
            } //default unmaintain value

    }

    int scanFrontalArea(GameObject Agent)
    {
        int counter = 0;
        foreach (GameObject Gazing in GameObject.FindGameObjectsWithTag("Gazing") ) //scanning for gazers
            {
            if (Vector3.Distance(Agent.transform.position, Gazing.transform.position) <= transferThreshold) //if any gazer within range
                if (Vector3.Angle (Agent.transform.forward, Gazing.transform.position - Agent.transform.position) <= 80 ) //within visual area
                  counter++;
            }

        return counter; 
        //returns amount of gazing agents in one's visual area
    }
}
