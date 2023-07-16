using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public float rotationSpeed = 10f; // D�n�� h�z�, ihtiyaca g�re ayarlanabilir
    public float targetAngle = -70f; // Hedef a��, ihtiyaca g�re ayarlanabilir
    private float currentAngle = 0f;

    public void Start()
    {
        StartCoroutine(RotateDoor());
    }

    private IEnumerator RotateDoor()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 70, transform.rotation.eulerAngles.z);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
    }
}
