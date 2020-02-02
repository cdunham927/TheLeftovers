using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    Character player;

    private void Awake()
    {
        player = FindObjectOfType<Character>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == player.gameObject)
        {
            //Debug.Log(player);
            player.healCharacter(4);
            this.gameObject.SetActive(false);
        }
    }
}
