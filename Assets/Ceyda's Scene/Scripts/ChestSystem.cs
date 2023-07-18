using UnityEngine;
using System.Collections;

public class ChestSystem : MonoBehaviour
{
    public Transform lid; // Kapak nesnesi
    public float openAngle = 50f; // Açýlacak açý (90 derece gibi)
    public float openSpeed = 2f; // Açýlma hýzý

  //  public ParticleSystem levelUpEffect;

    private bool isPlayerNearby = false;
    private bool isChestOpen = false;
    private Quaternion initialRotation;

    private void Start()
    {
        // Baþlangýçta kapak nesnesinin baþlangýç rotasyonunu kaydedin
        initialRotation = lid.transform.localRotation;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isChestOpen)
        {
            Debug.Log("Girdi");
            OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void OpenChest()
    {
        // Kapaklarý yavaþça açmak için döndürecek Quaternion rotasyonunu oluþturun
        Quaternion targetRotation = Quaternion.Euler(-openAngle,0 , 0f) * initialRotation;

        // Quaternion rotasyonunu kullanarak kapaklarý yavaþça açýn
        StartCoroutine(RotateLid(targetRotation));

        /*  if (openEffect != null)
          {
              levelUpEffect.Play();
          }   */

        isChestOpen = true;
    }

    private IEnumerator RotateLid(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion initialRotation = lid.transform.localRotation;

        while (elapsedTime < openSpeed)
        {
            lid.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / openSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Sonunda tam açýya gelmesi için emin olalým
        lid.transform.localRotation = targetRotation;
    }
}