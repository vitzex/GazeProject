using UnityEngine;
using System.Collections;

public class IdleRunJump : MonoBehaviour {


	protected Animator animator;
    public GameObject threshold, Gaze, headtest;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true;
   int gazeCounter = 50;
    public int counter;
    public int transferThreshold = 7;

   // public int gazersInSight(GameObject Agent)
   // {
   //    if 
   //
   //  }

    // Use this for initialization
    void Start () 
	{
		animator = GetComponent<Animator>();
		
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);

        Gaze = GameObject.FindGameObjectWithTag("Gaze");

        // transferThreshold = 7;

        //threshold = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //      threshold.GetComponent<Collider>().enabled = false;
        //    threshold.transform.position = Gaze.transform.parent.transform.position - new Vector3(0, Gaze.transform.parent.transform.position.y, 0);
        //  threshold.transform.localScale = new Vector3(2 * transferThreshold, 0.1f, 2 * transferThreshold);

        // Quaternion rot = Quaternion.Euler(Random.Range(0, 80), Random.Range(-80, 80), Random.Range(0, 80));
        Gaze.transform.parent.transform.localRotation = Quaternion.Euler(Random.Range(0, 80), Random.Range(-80, 80), Random.Range(0, 80));
        //can rotate -80 to 80 sideways (y rotation
        // can rotate 0 to 80 up-down (x, z rotation) so that others can stare up too (not be blocked)
        //only works when joint_head copied outside of neck -> parent now torso
    }

    void OnGUI()
    {
        counter = 0;

    //     foreach (UnityEngine.GameObject x in UnityEngine.GameObject.FindGameObjectsWithTag("Gazing") )
        //foreach (UnityEngine.GameObject x in UnityEngine.Object.FindObjectsOfType<GameObject>())
         //   counter = counter++;

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label(counter + " gazing");

    }

    // Update is called once per frame
    void Update () 
	{




        if (gazeCounter == 0)
        {
          //  GameObject.Find("RotateTowardsMe").transform.localRotation = Quaternion.Euler(Random.Range(0, 80), Random.Range(-80, 80), Random.Range(0, 80));
            Gaze.transform.parent.transform.localRotation = Quaternion.Euler(Random.Range(0, 80), Random.Range(-80, 80), Random.Range(0, 80));
            gazeCounter = 50;
            //localRotation so it's fine to re-rotate as it will still land close to parent
        }
        else
        {
            gazeCounter--;
         //   Quaternion.RotateTowards(Gaze.transform.parent.transform.rotation, GameObject.Find("RotateTowardsMe").transform.rotation, 1000);
            //Quaternion.RotateTowards(Gaze.transform.parent.transform.rotation, GameObject.Find("rotateTowardsMe").transform.rotation, 1);
        }//updating gaze from time to time
      //  Debug.Log(gazeCounter);

        foreach (GameObject headtest in GameObject.FindGameObjectsWithTag("Test"))
        headtest.transform.forward = Vector3.RotateTowards(headtest.transform.forward, Gaze.transform.position - headtest.transform.position, 0.02f, 0);

      //  threshold.transform.position = Gaze.transform.parent.transform.position - new Vector3(0, Gaze.transform.parent.transform.position.y, 0);
        //threshold area outlined - in case gaze.parent moves

        //   foreach (GameObject Agent in GameObject.FindGameObjectsWithTag("Agent")) //scanning for gazers
        ///   foreach (GameObject Gazing in GameObject.FindGameObjectsWithTag("Gazing")) //scanning for gazers
        //    if (Vector3.Distance(Agent.transform.position, Gazing.transform.position) <= 7)
        //   {
        // gazersInSight(Agent);
        //   }

        if (animator)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			

			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (Input.GetButton("Fire1")) animator.SetBool("Jump", true);                
            }
			else
			{
				animator.SetBool("Jump", false);                
            }

			if(Input.GetButtonDown("Fire2") && animator.layerCount >= 2)
			{
				animator.SetBool("Hi", !animator.GetBool("Hi"));
			}
			
		
      		float h = Input.GetAxis("Horizontal");
        	float v = Input.GetAxis("Vertical");

            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);	
		}   		  
	}
}
