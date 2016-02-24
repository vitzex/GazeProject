using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour {

    public bool overwrite;
    GameObject Gaze, Agent, Clone, threshold;
    float angle, transferThreshold;

	// Use this for initialization
	void Start () {

        Agent = gameObject; //the object being called

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
            //    angle = Vector3.Angle(Gaze.transform.position - Agent.transform.position, Agent.transform.forward);

            //   Vector3 dist = Gaze.transform.position - Gaze.transform.position - gameObject.transform.position;
            //     Vector3 desired = dist.normalized * maxSpeed;
            //     gameObject.transform.forward = desired; // - gameObject.transform.forward; //forward is basically our velocity

            /////STUFF BEFORE

            gameObject.transform.forward = Gaze.transform.position - gameObject.transform.position;

            //Clone = Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0) );
            // GameObject.Find(name).transform.rotation = Quaternion.Euler(Gaze.transform.position);
            //Debug.Log(angle);
            //gameObject.transform.localScale = Gaze.transform.position;
            // Clone.transform.position = gameObject.transform.position;
            //   Clone.transform.forward = Gaze.transform.position - Clone.transform.position;
            //localrotation - relativ4e to the parent's rotation  

            //  Debug.DrawLine(Agent.transform.position, Gaze.transform.position, Color.red,1000, true); 
        }
        else gameObject.transform.forward = gameObject.transform.parent.transform.forward; //chest forward

	}
}
