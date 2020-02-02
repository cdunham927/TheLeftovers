using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float xSpd;
    public float ySpd;
    public float zSpd;

    void Update()
    {
        transform.Rotate(xSpd * Time.deltaTime, ySpd * Time.deltaTime, zSpd * Time.deltaTime);

    }

}
