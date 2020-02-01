using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;

    public float horizontalMove = 0f;
 

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        //if (Input.GetButtonDown)
    }

     void FixedUpdate()
    {
        //Move.(horizontalMove * Time.fixedDeltaTime, false, jump);
    }
}
