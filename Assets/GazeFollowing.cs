using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour {
    public enum State { Stroll, Decide, Follow }

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
        Debug.Log(_state);
    }

    public bool overwrite;
    GameObject Gaze, Agent, Clone, threshold, target;
    float angle, transferThreshold, stepRadians, randomVar = 10;
    int visible = 0, prev_visible=0;

    // Use this for initialization
    void Start () {

        Agent = gameObject; //the object being called

        stepRadians = 7* Mathf.PI / 180;

     //   defaultSpd=500;
     //   maxSpeed = defaultSpd;
     //    maxSpeed = Mathf.Pow(0.02f,2)*Time.deltaTime;

        Gaze = GameObject.FindGameObjectWithTag("Gaze");

      //  {
          //  Quaternion rot = Quaternion.Euler(Random.Range(0, 80), Random.Range(-80, 80), Random.Range(0, 80));
          //  Gaze.transform.parent.transform.localRotation = rot;
         //   Debug.Log("How many times?");
    //    }
    //triggered everytime a creature spawns => move to permanent script


      

        transferThreshold = 7;

        threshold = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        threshold.GetComponent<Collider>().enabled = false;
        threshold.transform.position = Gaze.transform.parent.transform.position - new Vector3(0,Gaze.transform.parent.transform.position.y, 0);
        threshold.transform.localScale = new Vector3(2*transferThreshold, 0.1f, 2*transferThreshold);

        //  Clone = (GameObject) Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0));
    }
	
	// Update is called once per frame
	void Update () {
        threshold.transform.position = Gaze.transform.parent.transform.position - new Vector3(0, Gaze.transform.parent.transform.position.y, 0); //in case gaze.parent moves
        //  Gaze.transform.rotation = Random.rotation;

        if (Vector3.Distance(Agent.transform.position, Gaze.transform.parent.transform.position) <= transferThreshold)
        {
            Vector3 desired = Gaze.transform.position - Agent.transform.position;

          if (Vector3.Angle(gameObject.transform.forward, desired) <80) // check if within visual field)
            {
                if (Vector3.Angle(gameObject.transform.parent.transform.forward, desired) <= 135) 
                    //(physically harder to look back over 135 degrees
                    gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, desired, stepRadians, 0);
                    //rotate towards target
                    //steering behaviour -> normalized by stepRadians

                else gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, gameObject.transform.parent.transform.forward, stepRadians, 0);
                //if not within physically comfortable angle, rotate back towards run direction
            }

            // gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(Gaze.transform.position - Agent.transform.position), 5);
            //rotation syntax seems to yield wierder movements.

            //****** Bits of code from unsuccessful tries using other functions than rotate towards:
            //  angle = Vector3.Angle(Gaze.transform.position - Agent.transform.position, Agent.transform.forward);

            //  if (maxSpeed>0)
            //if (gameObject.transform.forward.normalized != desired.normalized)
            //  float desired = angle * maxSpeed;
            // gameObject.transform.Rotate( (Gaze.transform.position - Agent.transform.forward) );
            //Vector3 desired = gameObject.transform.forward + (Gaze.transform.position - gameObject.transform.forward) / maxSpeed;


            //gameObject.transform.forward = desired.normalized;

            // Vector3 desired = ( Gaze.transform.position + gameObject.transform.forward - 2 * gameObject.transform.position) / 2;
            //gameObject.transform.forward = desired;
            //  maxSpeed = maxSpeed - 1;
            //  Vector3 dist = Gaze.transform.position - gameObject.transform.position;
            //    Vector3 desired = dist.normalized * maxSpeed;
            //gameObject.transform.forward = desired; // - gameObject.transform.forward; //forward is basically our velocity

            //gameObject.transform.forward = Gaze.transform.position - gameObject.transform.position;


            /////STUFF BEFORE


            //Clone = Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0) );
            // GameObject.Find(name).transform.rotation = Quaternion.Euler(Gaze.transform.position);
            //Debug.Log(angle);
            //gameObject.transform.localScale = Gaze.transform.position;
            // Clone.transform.position = gameObject.transform.position;
            //   Clone.transform.forward = Gaze.transform.position - Clone.transform.position;
            //localrotation - relativ4e to the parent's rotation  

            //  Debug.DrawLine(Agent.transform.position, Gaze.transform.position, Color.red,1000, true); 
        }
        else {


            if ( (gameObject.transform.forward.normalized != gameObject.transform.parent.transform.forward.normalized) && (randomVar > 0) )
            {
                float k = 0.007f;
                 gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, gameObject.transform.parent.transform.forward + new Vector3(Random.Range(-k,k), Random.Range(-k, k), Random.Range(-k, k) ), stepRadians, 0 );
                randomVar--;
              //  if (randomVar == 0)
                    
                // target = (GameObject) gameObject.transform.parent;
                //    target.transform.Rotation = Random.Range(-20, 20));
                //wait another randomvariable * time.deltaTime;
                //randomvariable -- 
            }
            else
            {
                gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, gameObject.transform.parent.transform.forward, stepRadians, 0);
                randomVar = 10 ;
            }
            // gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(gameObject.transform.parent.transform.forward), 5);
            //Rotation code too ^


            //****** Unsuccessful tries using other functions than rotate towards:
            //gameObject.transform.forward = gameObject.transform.parent.transform.forward; //chest forward
            //  gameObject.transform.forward = gameObject.transform.forward.normalized;
            //maxSpeed = defaultSpd;

            StartCoroutine(States(gameObject)); //determine state of the agent


        }
    }

    IEnumerator States (GameObject Agent)
    {
        SetState(_state);

        while (true)
        {
            switch (_state)
            {
                case State.Stroll:
                    gazeAround(Agent);

                    prev_visible = visible;
                    visible = scanFrontalArea (Agent)  - do it as int function 
                    // if (visible > old_visible) 
                    //{
                      //  _prev_state = _state;
                        //       _state = State.Decision; }

                        //if blabla { _prev_state= _state;
                        //       _state = State.praaa;
                        //}

                        break; 
               
            }
            yield return null;
        }

    }

    void gazeAround(GameObject Agent)
    {
        Agent.transform.forward = Vector3.RotateTowards(Agent.transform.forward, Agent.transform.parent.transform.forward, stepRadians, 0);
    }

    int scanFrontalArea(GameObject Agent)
    {
        int counter = 0;
        foreach (GameObject Gazing in GameObject.FindGameObjectsWithTag("Gazing") )
            {
            if (Vector3.Distance(Agent.transform.position, Gazing.transform.position) <= transferThreshold)
                if (Vector3.Angle (Agent.transform.forward, Gazing.transform.position) <= 80 ) //within visual area
                  counter++;
            }

        return counter; 
        //returns amount of gazing agents in one's visual area
    }
}
