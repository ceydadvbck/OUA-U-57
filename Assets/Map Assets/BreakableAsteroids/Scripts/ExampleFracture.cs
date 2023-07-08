using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleFracture : MonoBehaviour
{
    public GameObject[] asteroids;

    private int counter = 0;

    void Update()
    {
        //Code loops through asteroids and fractures them on space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            asteroids[counter].GetComponent<Fracture>().FractureObject();
            counter++;
        }
    }

}
