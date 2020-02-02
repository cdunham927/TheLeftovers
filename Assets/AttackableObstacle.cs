using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObstacle : MonoBehaviour
{
    public int health;
    public float particleLastTime = 1f;
    public Transform stoneParticle;

    private void Start()
    {
        if(stoneParticle == null)
        {
            Debug.LogError("AttackableObstacle: no stone particle found");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Transform particleClone = Instantiate(stoneParticle, transform.position, transform.rotation);
            var mainModule = particleClone.GetComponent<ParticleSystem>().main;
            mainModule.startColor = transform.GetComponent<SpriteRenderer>().color;
            Destroy(transform.gameObject);
            Destroy(particleClone.gameObject, particleLastTime);
        }
    }
}
