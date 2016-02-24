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
            angle = Vector3.Angle(Gaze.transform.position - Agent.transform.position, Agent.transform.forward);

            //Clone = Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0) );
            // GameObject.Find(name).transform.rotation = Quaternion.Euler(Gaze.transform.position);
            //Debug.Log(angle);
            //gameObject.transform.localScale = Gaze.transform.position;
            // Clone.transform.position = gameObject.transform.position;
            //   Clone.transform.forward = Gaze.transform.position - Clone.transform.position;
            //localrotation - relativ4e to the parent's rotation  
            gameObject.transform.forward = Gaze.transform.position - gameObject.transform.position;
            //  Debug.DrawLine(Agent.transform.position, Gaze.transform.position, Color.red,1000, true); 
        }
        else gameObject.transform.forward = gameObject.transform.parent.transform.forward; //chest forward

	}
}
