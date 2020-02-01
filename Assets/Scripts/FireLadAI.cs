using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLadAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange;
    public float moveSpeed;
    
    public enum FireLadStates {
        Idle = 0,
        Tracking = 1
    };

    public FireLadStates FireLadState = FireLadStates.Idle;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fire Lad AI script started (FireLadAI.cs)");
        FireLadState = FireLadStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (FireLadState) { //transitions
            case FireLadStates.Idle:
                if (Vector3.SqrMagnitude(player.position - transform.position) <= detectionRange * detectionRange) {
                    FireLadState = FireLadStates.Tracking;
                    Debug.Log("Fire Lad sees the player");
                }
                break;
            case FireLadStates.Tracking:
                if (Vector3.SqrMagnitude(player.position - transform.position) > detectionRange * detectionRange) {
                    FireLadState = FireLadStates.Idle;
                    Debug.Log("Fire Lad doesn't see the player");
                }
                break;
            default:
                break;
        }

        switch (FireLadState) { //action
            case FireLadStates.Idle:
                //do idle behavior
                break;
            case FireLadStates.Tracking:
                break;
            default:
                break;
        }
    }
}
