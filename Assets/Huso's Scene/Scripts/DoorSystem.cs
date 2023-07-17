using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public float rotationSpeed = 10f; // Dönüþ hýzý, ihtiyaca göre ayarlanabilir
    public float targetAngle = 70f; // Hedef açý, ihtiyaca göre ayarlanabilir

    bool doorOpened;

    public void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") &&!doorOpened)
        {
            StartCoroutine(RotateDoor());
            doorOpened = true;

        }
    }
    private IEnumerator RotateDoor()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - targetAngle, transform.rotation.eulerAngles.z);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
    }
}
