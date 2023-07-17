using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryControl : MonoBehaviour
{
    [SerializeField] GameObject libraryDoor;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("Player"))
        {
            libraryDoor.GetComponent<Animator>().SetTrigger("LibraryDoor");
        }
    }
}
