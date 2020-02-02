using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    //Variables
    public float speed = 1f;
    public float jumpSpeed = 10f;
    Rigidbody2D bod;
    Animator anim;
    SpriteRenderer rend;
    Character play;

    public Vector2 boxRayCast;
    public float box;
    public bool grounded;
    public LayerMask mask;

    public GameObject projSpawn;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        play = GetComponent<Character>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (h != 0)
        {
            anim.SetBool("run", true);
        }
        else anim.SetBool("run", false);
        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        rend.flipX = (mousePos.x > 0.5f) ? true : false;
        projSpawn.transform.localPosition = new Vector3((mousePos.x > 0.5f) ? 0.25f : -0.25f, projSpawn.transform.localPosition.y, projSpawn.transform.localPosition.z);

        bod.AddForce(new Vector2(h * speed * Time.deltaTime, 0));

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            bod.AddForce(Vector2.up * jumpSpeed);
        }
        Collider2D hit;
        if (hit = Physics2D.OverlapBox(transform.position, boxRayCast, 0f, mask))
        {
            grounded = true;
        }
        else grounded = false;

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (play.getHealth() > 0) play.takeDamage(1);
            }
        }

        anim.SetBool("grounded", grounded);
        anim.SetBool("death", (play.getHealth() > 0) ? false : true);
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
