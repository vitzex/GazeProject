 /// <summary>
/// 
/// </summary>

using UnityEngine;
using System;
using System.Collections;
  
[RequireComponent(typeof(Animator))]  

//Name of class must be name of file as well

public class RandomCharacters : MonoBehaviour {
	
	public float AvatarRange = 1;

	protected Animator avatar;
	
	private float SpeedDampTime = 1.2f;	//increasing global damp time will make agents stagnate more
	private float DirectionDampTime = 2;	
	private Vector3 TargetPosition = new Vector3(0,0,0);
	
	// Use this for initialization
	void Start () 
	{
		avatar = GetComponent<Animator>();
	}
    
	void Update () 
	{
		if(avatar)
		{
          //  int rand = UnityEngine.Random.Range(0, 50);

          //  avatar.SetBool("Jump", rand == 20); //no more gimmicks
          //  avatar.SetBool("Dive", rand == 30);
			
			if(Vector3.Distance(TargetPosition,avatar.rootPosition) > 5)
			{
				avatar.SetFloat("Speed",0.3f,3.5f*SpeedDampTime, Time.deltaTime);

                avatar.speed = 0.5f;
                //speed fix

                Vector3 curentDir = avatar.rootRotation * Vector3.forward;
				Vector3 wantedDir = (TargetPosition - avatar.rootPosition).normalized;

                //dot prod = length of the projection of curentDir onto desiredDir multiplied by the length of desiredDir  (or vice versa)
                // => dot prod < 0 means different "orientations" i.e. angle >90 degrees (cos negative) (going "different ways")

                if (Vector3.Dot(curentDir,wantedDir) > 0)
				{
                    //avatar.SetFloat("Direction", wantedDir.y, DirectionDampTime, Time.deltaTime); //checking
                    avatar.SetFloat("Direction",Vector3.Cross(curentDir,wantedDir).y,DirectionDampTime, Time.deltaTime);
                    //go towards cross product .y = curentDir.z*wantedDir.x - curentDir.x*wantedDir.z ;
                }
                else
				{
                    //avatar.SetFloat("Direction", wantedDir.y, DirectionDampTime, Time.deltaTime); //checking
                    avatar.SetFloat("Direction", Vector3.Cross(curentDir,wantedDir).y > 0 ? 1 : -1, DirectionDampTime, Time.deltaTime);
                    //direction = sign of Cross product [ if dot product < 0]
                    //thus turn around when going "opposite" ways (angle > 90 degrees) ?
                }
			}
			else
			{
                avatar.SetFloat("Speed", 0, SpeedDampTime, Time.deltaTime);
				
				if(avatar.GetFloat("Speed") < 0.01f)
				{
					TargetPosition = new Vector3(UnityEngine.Random.Range(-AvatarRange,AvatarRange),0,UnityEngine.Random.Range(-AvatarRange,AvatarRange));
				}
			}
		}		
	}   		  
}
