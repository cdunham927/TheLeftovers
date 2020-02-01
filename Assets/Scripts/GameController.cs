using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> checkpoints;
    [SerializeField]
    public List<GameObject> monsters;
    private int cur_checkpoint = default;
    public Character player;

    public void Die()
    {
        Vector3 x = new Vector3(checkpoints[cur_checkpoint].transform.localPosition.x, -1.5f, 0);
        player.transform.localPosition = x;
        player.setHealth(player.getMaxHealth());
        //reinitialize enemies at original positions
    }

    public void handleCheckpoint(ref Collider2D col)
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (col.name == checkpoints[i].name)
            {
                Debug.Log(i);
                if (cur_checkpoint < i)
                {
                    cur_checkpoint = i;
                }
            }
        }
    }
}
