 /// <summary>
/// 
/// </summary>

using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

//Name of class must be name of file as well

public class RandomCharacters : MonoBehaviour
{

    public float AvatarRange = 1;

    protected Animator avatar;
    float randomized;

    private float SpeedDampTime = 1.2f; //increasing global damp time will make agents stagnate more
    private float DirectionDampTime = 2;
    private Vector3 TargetPosition = new Vector3(3, 0, 17), prevTarget, TargetPos;

    public enum State { upwards, walk, downwards };

    bool check = false; //checking for state swithc
    State _state = State.upwards;
   public State _prevState;

    #region Basic Getters/Setters
    public State CurrentState
    {
        get { return _state; }
    }

    public State PrevDirState
    {
        get { return _prevState; }
    }
    #endregion

    public void SetDirState(State newState)
    {
        _prevState = _state;
        _state = newState;
        //   Debug.Log(_state); 
        // working correctly atm so commented out
    }

    // Use this for initialization
    void Start()
    {
        _prevState = State.downwards;
        SetDirState(State.upwards);
        avatar = GetComponent<Animator>();
        randomized = UnityEngine.Random.Range(0.2f, 0.7f);
        TargetPos = TargetPosition + new Vector3(UnityEngine.Random.Range(-7, 7), 0, 0);
    }

    void Update()
    {
        if (avatar)
        {
            //  int rand = UnityEngine.Random.Range(0, 50);

            //  avatar.SetBool("Jump", rand == 20); //no more gimmicks
            //  avatar.SetBool("Dive", rand == 30);

            if (Vector3.Distance(TargetPos, avatar.rootPosition) > 1)
            {
                avatar.SetFloat("Speed", 0.3f, 3.5f * SpeedDampTime, Time.deltaTime);

                avatar.speed = randomized;
                //speed fix

                Vector3 curentDir = avatar.rootRotation * Vector3.forward;
                Vector3 wantedDir = (TargetPos - avatar.rootPosition).normalized;

                //dot prod = length of the projection of curentDir onto desiredDir multiplied by the length of desiredDir  (or vice versa)
                // => dot prod < 0 means different "orientations" i.e. angle >90 degrees (cos negative) (going "different ways")

                if (Vector3.Dot(curentDir, wantedDir) > 0)
                {
                    //avatar.SetFloat("Direction", wantedDir.y, DirectionDampTime, Time.deltaTime); //checking
                     avatar.SetFloat("Direction", Vector3.Cross(curentDir, wantedDir).y, DirectionDampTime, Time.deltaTime);
                    //avatar.SetFloat("Direction", Mathf.Lerp( 0, Vector3.Angle(curentDir, wantedDir), UnityEngine.Random.Range(0.9f,1f) ), DirectionDampTime, Time.deltaTime);
                    //  avatar.SetFloat("Direction", Vector3.Lerp(curentDir, wantedDir, UnityEngine.Random.Range(0f,1f) ), DirectionDampTime, Time.deltaTime);
                    //go towards cross product .y = curentDir.z*wantedDir.x - curentDir.x*wantedDir.z ;
                }
                else
                {
                    //avatar.SetFloat("Direction", wantedDir.y, DirectionDampTime, Time.deltaTime); //checking
                    avatar.SetFloat("Direction", Vector3.Cross(curentDir, wantedDir).y > 0 ? 1 : -1, DirectionDampTime, Time.deltaTime);
                    //direction = sign of Cross product [ if dot product < 0]
                    //thus turn around when going "opposite" ways (angle > 90 degrees) ?
                }
            }
            else
            {
                check = true;
                  StartCoroutine(DirectionState(avatar));
   

                // if (_prevState==State.downwards) SetState(State.upwards);
                // else SetState(State.downwards);
            //    if (TargetPosition == new Vector3(3, 0, 17))

          //      {
             //       prevTarget = TargetPosition;
            //        TargetPosition = new Vector3(-9, 0, -18);
                   
             //   }
           //     else

             //   {
              //      prevTarget = TargetPosition;
              //      TargetPosition = new Vector3(3, 0, 17);

               // }

               // TargetPos = TargetPosition + new Vector3(UnityEngine.Random.Range(-7, 7), 0, 0);

                avatar.SetFloat("Speed", 0, SpeedDampTime, Time.deltaTime);
               // gameObject.transform.position = prevTarget + new Vector3(UnityEngine.Random.Range(0, 7), 0, UnityEngine.Random.Range(0, 7));
                // gameObject.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-5, 5), 0);

                //Spawner.count--;

                if (avatar.GetFloat("Speed") < 0.01f)
                {
                    Destroy(gameObject);
                    //TargetPosition = new Vector3(UnityEngine.Random.Range(-AvatarRange,AvatarRange),0,UnityEngine.Random.Range(-AvatarRange,AvatarRange));
                }
            }
        }
    }

     IEnumerator DirectionState(Animator avatar)
     {
    // SetState(_state);

          while (true)
         {
           switch (_state)
           {
              case State.upwards:
                    //Agent.transform.forward = Agent.transform.parent.transform.forward; 
                    //no need, no one can see :

                    prevTarget = TargetPosition;

                    TargetPosition = new Vector3(3, 0, 17);
                    TargetPos = TargetPosition + new Vector3(UnityEngine.Random.Range(-7, 7), 0, 0);
                    randomized = UnityEngine.Random.Range(0.2f, 0.7f);
                    avatar.speed = randomized;
                    //  gameObject.tag = "upwards";

                    // Debug.Log("upwards");

                    SetDirState(State.walk);
          break;

        case State.walk:
     if ( (check==true)&&(_prevState==State.downwards) ) {check = false; SetDirState(State.upwards); yield break; }
    if ( (check==true)&&(_prevState==State.upwards) ) {check = false;  SetDirState(State.downwards); yield break; }

    for (int k=1; k<=7; k++)
     if ( Vector3.Distance(TargetPos, avatar.rootPosition) == 5*k)
                        TargetPos = TargetPosition + new Vector3(UnityEngine.Random.Range(-7, 7), 0, 0);
    //periodic direction changes
                    break;

      case State.downwards:

                    prevTarget = TargetPosition;

                    TargetPosition = new Vector3(-9, 0, -18);
                    TargetPos = TargetPosition + new Vector3(UnityEngine.Random.Range(-7, 7), 0, 0);
                    randomized = UnityEngine.Random.Range(0.2f, 0.7f);
                    avatar.speed = randomized;
                    // gameObject.tag = "downwards";

                    //   Debug.Log("downwards");

                    SetDirState(State.walk);
      break;
     }
      }
    }

}
