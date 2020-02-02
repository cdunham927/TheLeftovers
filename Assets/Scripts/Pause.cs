using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject paws;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0f;
                paws.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                paws.SetActive(false);
            }

        }
    }
    
}
