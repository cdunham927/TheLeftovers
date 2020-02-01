using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int cur_health = 12;
    [SerializeField]
    private int max_health = 12;

    public int getHealth() {
        return cur_health;
    }

    public void setHealth(int x) {
        cur_health = x;
        return;
    }

    public int getMaxHealth() {
        return max_health;
    }

    public void setMaxHealth(int x) {
        max_health = x;
        return;
    }

    public void takeDamage(int d) {
        cur_health -= d;
        if(cur_health <= 0) {
            cur_health = 0;
            //gamecontroller.hasDied();
        }
        return;
    }

    public void healCharacter(int h) {
        cur_health += h;
        if(cur_health >= max_health) {
            cur_health = max_health;
        }
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
