using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip takeDamageSound_fire;
    public AudioClip takeDamageSound_water;
    public AudioClip takeDamageSound_thunder;
    public AudioClip takeDamageSound_ice;
    public AudioClip takeDamageSound_earth;

    //Can Add additional clips

    public AudioSource sound;

    public void Awake()
    {
        sound.clip = takeDamageSound_fire;
    }

    public void enemyTakeDamage(string EnemyType)
    {
        if (EnemyType == "Fire")
        {
            sound.clip = takeDamageSound_fire;
            sound.Play();
        }
        else if (EnemyType == "Water")
        {
            sound.clip = takeDamageSound_water;
            sound.Play();
        }
        else if (EnemyType == "Thunder")
        {
            sound.clip = takeDamageSound_thunder;
            sound.Play();
        }
        else if (EnemyType == "Ice")
        {
            sound.clip = takeDamageSound_ice;
            sound.Play();
        }
        else if (EnemyType == "Earth")
        {
            sound.clip = takeDamageSound_earth;
            sound.Play();
        }
    }
}
