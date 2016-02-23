using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour {

    public bool overwrite;
    GameObject Gaze, Agent, Clone;
    float angle;

	// Use this for initialization
	void Start () {
      //  Clone = (GameObject) Instantiate(gameObject, gameObject.transform.position, Quaternion.Euler(0, angle, 0));
    }
	
	// Update is called once per frame
	void Update () {
        Agent = gameObject; //the object being called

        Gaze = GameObject.FindGameObjectWithTag("Gaze");

        //  Gaze.transform.rotation = Random.rotation;

        if (Vector3.Distance(Agent.transform.position, Gaze.transform.position) <= 7)
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
