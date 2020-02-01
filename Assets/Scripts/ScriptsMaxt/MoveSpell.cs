using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpell : MonoBehaviour
{
    public float force = 50f;
    public float maxDist = 50f;

    public Rigidbody2D rb;

    public Vector2 direction;
    private Vector3 _spawnLoc; //private veriable to store the initial position of this object

    private void Awake()
    {
        _spawnLoc = transform.position;
    }

    private void Start()
    {
        if(rb == null)
        {
            Debug.LogError("MoveSpell: no rigidbody 2d found");
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //move the spell if not reaching the max distance, destroy it if reaching max distance
        if ((transform.position - _spawnLoc).magnitude >= 50f)
        {
            Destroy(transform.gameObject);
        } else
        {
            rb.AddForce(direction * force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log("MoveSpell: we hit an enemy");
            //TODO: deal with effect
            Destroy(transform.gameObject);
        }
    }
}
