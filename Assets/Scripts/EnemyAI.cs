using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public SpriteRenderer sr;
    public CheckpointManager gm;
    private Vector3 orig_position;
    [SerializeField]
    private int checkpoint = default;
    [SerializeField]
    private int index; //used by gamemanager

    public int maxHealth;
    public int health;

    [Range(0, 1)]
    public float potionSpawnChance;
    public GameObject potion;

    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        gm = FindObjectOfType<CheckpointManager>();
        orig_position = transform.position;
    }

    public void TakeDamage(int amt)
    {
        health -= amt;
    }

    public void Die()
    {
        gm.EnemyDie(index);
        if (Random.value < potionSpawnChance)
        {
            Instantiate(potion, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }

    public void reinitialize()
    {
        transform.position = orig_position;
        health = maxHealth;
    }

    public int getEnemyCheckpoint()
    {
        return checkpoint;
    }

    public void setIndex(int i)
    {
        index = i;
    }
}
