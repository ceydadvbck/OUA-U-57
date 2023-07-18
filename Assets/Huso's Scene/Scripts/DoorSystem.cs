using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public float rotationSpeed = 2f; // Dönüþ hýzý, ihtiyaca göre ayarlanabilir
    public float targetAngle = -70f; // Hedef açý, ihtiyaca göre ayarlanabilir

    bool doorOpened;
    [SerializeField] GameObject Golems;
    [SerializeField] GameObject statueDoor;

    public void FixedUpdate()
    {
       
        if (Golems != null && Golems.transform.childCount == 0 && gameObject.transform.CompareTag("GolemDoor") && !doorOpened)
        {
            StartCoroutine(RotateDoor());
            doorOpened = true;
            Debug.Log("xd");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !doorOpened && !gameObject.transform.CompareTag("GolemDoor") && !gameObject.transform.CompareTag("Statue"))
        {
            StartCoroutine(RotateDoor());
            doorOpened = true;
        }
        if (other.gameObject.CompareTag("Player") && !doorOpened &&statueDoor != null && gameObject.transform.CompareTag("Statue"))
        {
            StartCoroutine(RotateDoor());
            doorOpened = true;
            statueDoor.AddComponent<DoorSystem>();
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
