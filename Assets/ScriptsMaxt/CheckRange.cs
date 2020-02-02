using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRange : MonoBehaviour
{
    public Transform player;
    public float maxDistToPickUp = 5f;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        Debug.DrawLine(transform.position, transform.position + Vector3.left * maxDistToPickUp, Color.blue);
        if(distToPlayer <= maxDistToPickUp)
        {
            player.GetComponent<PlayerAction>().ableToPickUp = true;
            transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            player.GetComponent<PlayerAction>().ableToPickUp = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
