using UnityEngine;
using System.Collections;

public class GazeFollowing : MonoBehaviour {

    GameObject Gaze, Agent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Agent = gameObject; //the object being called

        Gaze = GameObject.FindGameObjectWithTag("Gaze");

        if ( Vector3.Distance(Agent.transform.position, Gaze.transform.position) <=3)
         {
            Debug.DrawLine(Agent.transform.position, Gaze.transform.position, Color.red,1000, false); 
         }

	}
}
