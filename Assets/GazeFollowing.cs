using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour {

    public bool overwrite;
    GameObject Gaze, Agent, Clone, threshold;
    float angle, transferThreshold, stepRadians;

	// Use this for initialization
	void Start () {

        Agent = gameObject; //the object being called

        stepRadians = 5* Mathf.PI / 180;

     //   defaultSpd=500;
     //   maxSpeed = defaultSpd;
     //    maxSpeed = Mathf.Pow(0.02f,2)*Time.deltaTime;

        Gaze = GameObject.FindGameObjectWithTag("Gaze");
        Gaze.transform.parent.transform.localRotation = Quaternion.Euler(Random.Range(0, 80), Random.Range(-80, 80), Random.Range(0, 80));
        //can rotate -80 to 80 sideways (y rotation
        // can rotate 0 to 80 up-down (x, z rotation) so that others can stare up too (not be blocked)
        //only works when joint_head copied outside of neck -> parent now torso

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
 
       //  if (maxSpeed>0)
            if (gameObject.transform.forward.normalized != desired.normalized)
             {
                    gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward,Gaze.transform.position - Agent.transform.position,stepRadians,0);
             }
             
            //  angle = Vector3.Angle(Gaze.transform.position - Agent.transform.position, Agent.transform.forward);
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
        else { //gameObject.transform.forward = gameObject.transform.parent.transform.forward; //chest forward
          //  gameObject.transform.forward = gameObject.transform.forward.normalized;
            gameObject.transform.forward = Vector3.RotateTowards(gameObject.transform.forward, gameObject.transform.parent.transform.forward, stepRadians, 0);
            //maxSpeed = defaultSpd;
        }
	}
}
