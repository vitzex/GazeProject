using UnityEngine;
using System.Collections;

public class Spawner: MonoBehaviour {

    public GameObject prefabA;
    public GameObject prefabB;
   public int count = 0;
     float lastTime = 0;
    public int showCount = 0;
   // int counter = 0;


	void OnGUI()
	{

       // counter = 0;

     //   foreach (UnityEngine.GameObject x in UnityEngine.GameObject.FindGameObjectsWithTag("Gazing"))
      //      counter = counter++;

        GUILayout.Label("Spawns up to " + count + " characters");
	//	GUILayout.Label("Press fire button to spawn Teddy!");
      //  GUILayout.Label(counter + " gazing");
    }

    void Start()
    {
        lastTime = Time.time;
    }

    void Update()
    {
       

        if (count < 100)
        {
            bool alt = Input.GetButton("Fire1");

            if (Time.time - lastTime > 0.2f)
            {
                if(prefabA != null && !alt) Instantiate(prefabA, new Vector3( Random.Range(0,3), 0, -18 + Random.Range(0, 3)), Quaternion.Euler(0, Random.Range(-80, 80), 0 ));


                lastTime = Time.time;
                count++;
               showCount = count;
            }
        }


        if (Input.GetButton("Fire1") ) //mouse down - move things

        {
            Plane playerPlane = new Plane(Vector3.up, GameObject.FindGameObjectWithTag("Player").transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                Vector3 targetPoint = ray.GetPoint(hitdist); //code from online tutorial, as it's simpler than the Physics.raycast i had in mind               
               // Debug.Log(hitdist);
               Vector3 pos = ray.GetPoint(hitdist);
                GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(pos.x, 0, pos.z);
                GameObject.Find("Testing").transform.position = new Vector3(pos.x + 3, 0, pos.z);
                GameObject.Find("Testing (1)").transform.position = new Vector3(pos.x + 6, 0, pos.z);
                GameObject.Find("Testing (2)").transform.position = new Vector3(pos.x -3, 0, pos.z);
                //in case collider is higher
            }
        }
    }
}
