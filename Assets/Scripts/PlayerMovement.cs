using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    //Variables
    public float speed = 1f;
    public float jumpSpeed = 10f;
    Rigidbody2D bod;


    public Vector2 boxRayCast;
    public float box;
    public bool grounded;
    public LayerMask mask;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        bod.AddForce(new Vector2(h * speed * Time.deltaTime, 0));

        if (grounded && (Input.GetAxisRaw("Vertical") > 0 || Input.GetKeyDown(KeyCode.Space)))
        {
            bod.AddForce(Vector2.up * jumpSpeed);
        }
        Collider2D hit;
        if (hit = Physics2D.OverlapBox(transform.position, boxRayCast, 0f, mask))
        {
            grounded = true;
        }
        else grounded = false;

        Debug.Log(grounded);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        var left = transform.position - new Vector3(boxRayCast.x * 0.5f, 0f, 0f);
        var right = transform.position + new Vector3(boxRayCast.x * 0.5f, 0f, 0f);
        var down = -new Vector3(0f, boxRayCast.y, 0f);
        Gizmos.DrawLine(left, left + down);
        Gizmos.DrawLine(right, right + down);
        Gizmos.DrawLine(left, right);
        Gizmos.DrawLine(left + down, right + down);
    }
}
