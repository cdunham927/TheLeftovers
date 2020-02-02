using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public List<GameObject> checkpoints;
    public List<GameObject> enemies;
    private bool[] hasDied;
    [SerializeField]
    private int cur_checkpoint = default;
    public Transform player;
    public Sprite checkpointChecked;
    Character play;
    public GameObject restartUI;
    Potion[] potionSeller = new Potion[50];

    public void Awake()
    {
        play = FindObjectOfType<Character>();
        hasDied = new bool[enemies.Count];
        for(int i = 0; i < enemies.Count; i++)
        {
            hasDied[i] = false;
            var enemyai = enemies[i].GetComponentInChildren<EnemyAI>();
            enemyai.setIndex(i);
        }
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Die()
    {
        Time.timeScale = 1f;
        restartUI.SetActive(false);

        potionSeller = FindObjectsOfType<Potion>();
        foreach(Potion potion in potionSeller)
        {
            potion.gameObject.SetActive(false);
        }

        Vector3 x = new Vector3(checkpoints[cur_checkpoint].transform.localPosition.x, -1.5f, 0);
        player.localPosition = x;
        for(int i = 0; i < enemies.Count; i++)
        {
            if (hasDied[i])
            {
                enemies[i].SetActive(true);
            }
            var enemyai = enemies[i].GetComponentInChildren<EnemyAI>();
            if (enemyai.getEnemyCheckpoint() >= cur_checkpoint || !hasDied[i])
            {
                enemyai.reinitialize();
                hasDied[i] = false;
            }
            else if (hasDied[i])
            {
                enemies[i].SetActive(false);
            }
        }
        Debug.Log("Tried to restart");
        play.setHealth(play.getMaxHealth());
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                play.takeDamage(999);
            }
        }
    }

    public void EnemyDie(int i)
    {
        enemies[i].SetActive(false);
        hasDied[i] = true;
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        restartUI.SetActive(true);
    }

    public void handleCheckpoint(ref Collider2D col)
    {
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (col.name == checkpoints[i].name)
            {
                //Debug.Log(i);
                if(cur_checkpoint < i)
                {
                    cur_checkpoint = i;
                }
                checkpoints[cur_checkpoint].GetComponent<SpriteRenderer>().sprite = checkpointChecked;
            }
        }
    }
}
