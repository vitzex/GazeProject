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

    public enum State { upwards, downwards };

   // bool check = false; //checking for state swithc
    State _state = State.upwards;
    State _prevState;

    #region Basic Getters/Setters
    public State CurrentState
    {
        get { return _state; }
    }

    public State PrevState
    {
        get { return _prevState; }
    }
    #endregion

    public void SetState(State newState)
    {
        _prevState = _state;
        _state = newState;
        //   Debug.Log(_state); 
        // working correctly atm so commented out
    }

    // Use this for initialization
    void Start()
    {
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
                //  check = true;
                // StartCoroutine(DirectionState(avatar));

                if (TargetPosition == new Vector3(3, 0, 17))

                {
                    prevTarget = TargetPosition;
                    TargetPosition = new Vector3(-9, 0, -18);
                   
                }
                else

                {
                    prevTarget = TargetPosition;
                    TargetPosition = new Vector3(3, 0, 17);

                }

                TargetPos = TargetPosition + new Vector3(UnityEngine.Random.Range(-7, 7), 0, 0);

                avatar.SetFloat("Speed", 0, SpeedDampTime, Time.deltaTime);
               // gameObject.transform.position = prevTarget + new Vector3(UnityEngine.Random.Range(0, 7), 0, UnityEngine.Random.Range(0, 7));
                gameObject.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(-40, 40), 0);

                //Spawner.count--;

                if (avatar.GetFloat("Speed") < 0.01f)
                {
                    Destroy(gameObject);
                    //TargetPosition = new Vector3(UnityEngine.Random.Range(-AvatarRange,AvatarRange),0,UnityEngine.Random.Range(-AvatarRange,AvatarRange));
                }
            }
        }
    }

  //  IEnumerator DirectionState(Animator avatar)
  //  {
        // SetState(_state);

  //      while (true)
   //     {
    //        switch (_state)
     //       {
     //           case State.upwards:
                    //Agent.transform.forward = Agent.transform.parent.transform.forward; 
                    //no need, no one can see :

       //             if (check) {
         //               prevTarget = TargetPosition;

             //           TargetPosition = new Vector3(-9, 0, -18);
//
              //          avatar.transform.position = new Vector3(0, 0, 0);
            ///            check = false; SetState(State.downwards); }
            //
            //        break;
             //   case State.downwards:


               //     if (check)
               //     {
                  //      prevTarget = TargetPosition;

                   //     TargetPosition = new Vector3(3, 7, 22);

                  //      avatar.transform.position = new Vector3(0, 0, 0);
//
                 //       check = false; SetState(State.upwards);
                  //  }

                  //  break;
          // }
     //   }
   //}

}
