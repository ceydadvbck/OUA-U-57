using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMove : MonoBehaviour
{

    public float speed = 2.0f;
    private bool isRotatingDown = true;

    private void Update()
    {
        float rotationDelta = speed * Time.deltaTime;
        if (isRotatingDown)
        {
            transform.Rotate(Vector3.down, rotationDelta);
            if (transform.rotation.x <= -36.0f)
            {
                isRotatingDown = false;
            }
        }

    }

}
