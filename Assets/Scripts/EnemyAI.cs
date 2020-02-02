using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public SpriteRenderer sr;
    public CheckpointManager gm;
    public Vector3 orig_position;
    [SerializeField]
    public int checkpoint = default;
    [SerializeField]
    public int index; //used by gamemanager

    public int maxHealth;
    public int health;

    [Range(0, 1)]
    public float potionSpawnChance;
    public GameObject potion;

    public Collider2D coll;
    public Transform parent;
    public Rigidbody2D bod;

    public void TakeDamage(int amt)
    {
        health -= amt;
    }

    public void Die()
    {
        FindObjectOfType<CheckpointManager>().EnemyDie(index);
        if (Random.value < potionSpawnChance)
        {
            Instantiate(potion, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }

    public void reinitialize()
    {
        bod.velocity = Vector2.zero;
        parent.position = orig_position;
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
