using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineReflection : MonoBehaviour
{
    [SerializeField] private int numberOfRays;
    [SerializeField] private LineRenderer line;

    public void Start()
    {
        line.positionCount = numberOfRays + 1;
    }

    public void Update()
    {
        line.SetPosition(0, transform.position);
        CastRay(transform.position, transform.forward);
    }

    private void CastRay(Vector3 rayPos , Vector3 rayDir)
    {
        for (var i = 0; i < numberOfRays; i++)
        {
            var ray = new Ray(rayPos, rayDir);
            if (Physics.Raycast(ray,out var rayHit,15))
            {
                line.SetPosition(i + 1, rayHit.point);
                Debug.DrawLine(rayPos, rayHit.point,Color.red);
                rayPos = rayHit.point;
                rayDir = Vector3.Reflect(rayPos, rayHit.normal);

            }
            else
            {
                line.SetPosition(i + 1, rayDir * 15f);
                Debug.DrawRay(rayPos, rayDir * 15f, Color.red);
            }
        }
    }
}
