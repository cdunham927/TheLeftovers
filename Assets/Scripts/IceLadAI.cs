using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceLadAI : EnemyAI
{
    public Transform player;
    public GameObject enemy;
    public LayerMask environmentMask;
    public LayerMask playerMask;
    public float detectionRange = 7f;
    public float moveSpeed = 3f;

    public int attackStrength = 1;

    //Roaming behavior when idle
    public int minRoamTime = 2; //seconds
    public int maxRoamTime = 2; //seconds
    private int currentRoamTime;
    private float idleTimer;
    public float currentRoamSpeed;

    //jump
    public float jumpStrength = 0.2f;
    public bool grounded;

    public SpriteRenderer rend;

    public enum enemyStates
    {
        Idle = 0,
        Tracking = 1
    };

    void Awake()
    {
        player = FindObjectOfType<PlayerAction>().transform;
        coll = GetComponent<Collider2D>();
        gm = FindObjectOfType<CheckpointManager>();
        orig_position = parent.transform.position;
    }

    public enemyStates enemyState = enemyStates.Idle;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name == player.name)
        {
            player.GetComponent<Character>().takeDamage(attackStrength);
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveSpeed / 2, 3), ForceMode2D.Impulse);
            //Debug.Log(enemy.name + " hit " + player.name + " for " + attackStrength + " damage. " + player.name + " has " + player.GetComponent<Character>().getHealth() + " health remaining.");
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
        RaycastHit2D floorHit = Physics2D.Raycast(transform.position, new Vector2(0, -1f), 1f, environmentMask);
        RaycastHit2D playerHit = Physics2D.Raycast(transform.position, new Vector2(0, -1f), 1f, playerMask);
        if (floorHit.collider != null | playerHit.collider != null)
        {
            if (floorHit.distance < 0.01f | playerHit.distance < 0.01f)
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
        }

        if (health <= 0) Die();

        switch (enemyState)
        { //transitions
            case enemyStates.Idle:
                if (Vector3.SqrMagnitude(player.position - enemy.transform.position) <= detectionRange * detectionRange)
                {
                    enemyState = enemyStates.Tracking;
                    //Debug.Log("Fire Lad sees the player, now tracking.");
                }
                break;
            case enemyStates.Tracking:
                if (Vector3.SqrMagnitude(player.position - enemy.transform.position) > detectionRange * detectionRange)
                {
                    enemyState = enemyStates.Idle;
                    idleTimer = 0f;
                    //Debug.Log("Fire Lad doesn't see the player, now idle.");
                }
                break;
            default:
                enemyState = enemyStates.Idle;
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                TakeDamage(1);
            }
        }

        switch (enemyState)
        { //actions
            case enemyStates.Idle:
                idleTimer += Time.deltaTime;
                if (idleTimer >= currentRoamTime && grounded)
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
                        rend.flipX = true;
                        raycastDirection = new Vector2(1f, 0); //right
                    }
                    else
                    {
                        rend.flipX = false;
                        raycastDirection = new Vector2(-1f, 0); //left
                    }
                    rayHit = Physics2D.Raycast(enemy.transform.position, raycastDirection, 1f, environmentMask);
                    //Debug.DrawLine(enemy.transform.position, enemy.transform.position + (Vector3)raycastDirection, Color.red);
                    if (rayHit.collider != null)
                    {
                        if (rayHit.distance < 0.5f)
                        {
                            currentRoamSpeed *= -1;
                            idleTimer = 0f;
                            currentRoamTime = Random.Range(minRoamTime, maxRoamTime);
                        }
                    }
                }

                if (grounded)
                { //can jump
                    enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.up * jumpStrength, ForceMode2D.Impulse); //jump
                    enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.right * currentRoamSpeed); //move
                }

                break;
            case enemyStates.Tracking:
                //track player
                if (grounded)
                {
                    if (player.transform.position.x > enemy.transform.position.x)
                    {
                        rend.flipX = true;
                        //Debug.Log("Player is to the right of Fire Lad");
                        enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.right * moveSpeed); //move
                        if (moveSpeed <= 0) { moveSpeed *= -1; }
                    }
                    else if (player.transform.position.x < enemy.transform.position.x)
                    {
                        rend.flipX = false;
                        //Debug.Log("Player is to the left of Fire Lad");
                        //enemy.transform.position += new Vector3(-1 * moveSpeed * Time.deltaTime, 0, 0);
                        enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.right * moveSpeed); //move
                        if (moveSpeed > 0) { moveSpeed *= -1; }
                    }
                    enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.up * jumpStrength, ForceMode2D.Impulse); //jump
                }

                break;
            default:
                break;
        }
    }
}
