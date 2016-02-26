using UnityEngine;
using System.Collections;

public class Spawner: MonoBehaviour {

    public GameObject prefabA;
    public GameObject prefabB;
   public int count = 0;
     float lastTime = 0;
    public int showCount = 0;


	void OnGUI()
	{

		GUILayout.Label("Spawns up to " + count + " characters");
		GUILayout.Label("Press fire button to spawn Teddy!");		
	}

    void Start()
    {
        lastTime = Time.time;
    }

    void Update()
    {
        if(count < 40)
        {
            bool alt = Input.GetButton("Fire1");

            if (Time.time - lastTime > 0.2f)
            {
                if(prefabA != null && !alt) Instantiate(prefabA, new Vector3(Random.Range(0,3), 0, Random.Range(0, 3)), Quaternion.Euler(0, Random.Range(-80, 80), 0 ));
                if(prefabB != null && alt) Instantiate(prefabB, new Vector3(Random.Range(0, 3), 0, Random.Range(0, 3)), Quaternion.Euler(0, Random.Range(-80, 80), 0) );
                lastTime = Time.time;
                count++;
               showCount = count;
            }
        }
    }
}
