using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBossAI : EnemyAI
{
    public Transform player;
    public GameObject fireLad;
    public LayerMask environmentMask;
    public float detectionRange;
    public float moveSpeed;
    public int attackStrength;

    //Roaming behavior when idle
    public int minRoamTime; //seconds
    public int maxRoamTime; //seconds
    public int jumpChance; //modulo value
    private int currentRoamTime;
    private float idleTimer;
    public float currentRoamSpeed;

    //Tracking movement
    public int turnTime; //seconds

    //jump
    public int jumpStrength;
    private bool grounded;
    public Vector2 boxCast;
    public float box;

    public Image fireBossHp;
    public GameObject hpParent;
    public float bossfightDistance;
    public float lerpSpd;
    public GameObject scrollToDrop;

    public GameObject babies;
    [Range(1, 4)]
    public int babiesToSpawn;

    public enum FireLadStates
    {
        Idle = 0,
        Tracking = 1
    };

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        gm = FindObjectOfType<CheckpointManager>();
        orig_position = parent.transform.position;
    }

    public FireLadStates FireLadState = FireLadStates.Idle;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == player.name)
        {
            player.GetComponent<Character>().takeDamage(attackStrength);
            //Debug.Log("Fire Lad hit the player");
            //place function here to make player take damage
        }
    }

    void OnEnable()
    {
        currentRoamTime = Random.Range(minRoamTime, maxRoamTime);
        currentRoamSpeed = moveSpeed / 2;
        if (Random.Range(0, 1000) % 2 == 0)
        {
            currentRoamSpeed *= -1f;
        }
        health = maxHealth;
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D boxHit;
        if (boxHit = Physics2D.OverlapBox(fireLad.transform.position, boxCast, 0f, environmentMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        //sr.flipX = (currentRoamSpeed > 0) ? true : false;
        hpParent.SetActive((Vector3.SqrMagnitude(player.position - transform.position) <= bossfightDistance) ? true : false);
        fireBossHp.fillAmount = Mathf.Lerp(fireBossHp.fillAmount, (float)health / (float)maxHealth, lerpSpd * Time.deltaTime);

        switch (FireLadState)
        { //transitions
            case FireLadStates.Idle:
                if (Vector3.SqrMagnitude(player.position - fireLad.transform.position) <= detectionRange * detectionRange)
                {
                    FireLadState = FireLadStates.Tracking;
                    //Debug.Log("Fire Lad sees the player, now tracking.");
                }
                break;
            case FireLadStates.Tracking:
                if (Vector3.SqrMagnitude(player.position - fireLad.transform.position) > detectionRange * detectionRange)
                {
                    FireLadState = FireLadStates.Idle;
                    idleTimer = 0f;
                    //Debug.Log("Fire Lad doesn't see the player, now idle.");
                }
                break;
            default:
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(1);
            }
        }

        if (health < 1) {
            hpParent.SetActive(false);
            Instantiate(scrollToDrop, transform.position, Quaternion.identity);

            for (int i = 0; i < babiesToSpawn; i++)
            {
                Instantiate(babies, transform.position + new Vector3((i * 3) - 3, Random.Range(0, 2), 0), Quaternion.identity);
            }

            Die();
        }

        switch (FireLadState)
        { //actions
            case FireLadStates.Idle:
                idleTimer += Time.deltaTime;
                if (idleTimer >= currentRoamTime)
                {
                    currentRoamSpeed *= -1; //switch direction
                    idleTimer = 0f;
                    currentRoamTime = Random.Range(minRoamTime, maxRoamTime);
                }
                else
                {
                    //detect walls
                    RaycastHit2D rayHit;
                    Vector2 raycastDirection = new Vector2(0, 0);
                    if (currentRoamSpeed > 0)
                    {
                        raycastDirection = new Vector2(1f, 0); //right
                    }
                    else
                    {
                        raycastDirection = new Vector2(-1f, 0); //left
                    }
                    rayHit = Physics2D.Raycast(fireLad.transform.position, raycastDirection, 1f, environmentMask);
                    //Debug.DrawLine(fireLad.transform.position, fireLad.transform.position + (Vector3)raycastDirection, Color.red);
                    if (rayHit.collider != null)
                    {
                        if (rayHit.distance < 0.5f)
                        {
                            currentRoamSpeed *= -1;
                            idleTimer = 0f;
                            currentRoamTime = Random.Range(minRoamTime, maxRoamTime);
                        }
                    }

                    fireLad.transform.position += new Vector3(currentRoamSpeed * Time.deltaTime, 0, 0);
                }

                //only allow jumping while on floor
                /*
                Vector2 boxcastDirection = new Vector2(0, -1f);
                RaycastHit2D Boxcast2D
                boxHit = Physics2D.BoxCast(fireLad.transform.position, boxcastDirection, 1f, (1 << 8));
                if (boxHit.collider != null) { //jump while on floor
                    if (boxHit.distance < 0.1f) {
                        
                    }
                }
                */

                if (grounded)
                { //can jump
                    //fireLad.GetComponent<Rigidbody2D>().AddForce(fireLad.transform.up * jumpStrength, ForceMode2D.Impulse); //jump
                }

                break;
            case FireLadStates.Tracking:
                //track player and attack
                if (player.transform.position.x > fireLad.transform.position.x)
                {
                    sr.flipX = true;
                    //Debug.Log("Player is to the right of Fire Lad");
                    fireLad.transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                }
                else if (player.transform.position.x < fireLad.transform.position.x)
                {
                    sr.flipX = false;
                    //Debug.Log("Player is to the left of Fire Lad");
                    fireLad.transform.position += new Vector3(-1 * moveSpeed * Time.deltaTime, 0, 0);
                }

                /*
                if (player.transform.position.y > fireLad.transform.position.y) {
                    //jump
                }
                */
                break;
            default:
                break;
        }
    }
}
