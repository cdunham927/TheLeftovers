using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamage : MonoBehaviour
{
    public int Damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Untagged" || collision.tag == "Player")
        {
            return;
        }

        if (collision.tag == "Enemy")
        {
            //Debug.Log("MoveSpell: we hit an enemy");

            //TODO: deal with damage
            collision.GetComponent<EnemyAI>().TakeDamage(Damage);

            if (transform.gameObject.tag == "Shooted") //destroy the spell that is shooted by player. Lightning and earth are destroyed right after animation
            {
                Destroy(transform.gameObject);
            }
        } else
        {
            if(transform.gameObject.tag == "Shooted")
            {
                Destroy(transform.gameObject);
            }
        } 
    }
}
