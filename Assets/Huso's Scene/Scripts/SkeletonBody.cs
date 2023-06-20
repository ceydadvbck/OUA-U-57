using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBody : MonoBehaviour
{
    [SerializeField] SkeletonBody[] childLimbs;

    [SerializeField] GameObject limbPrefab;

    public void GetHit()
    {
        if (limbPrefab != null)
        {
            GameObject limbClone = Instantiate(limbPrefab, transform.position, transform.rotation);
            Destroy(limbClone, 5f);
        }

        transform.localScale = Vector3.zero;

        Destroy(gameObject);

    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.CompareTag("GroundAttackDamageBox"))
        {
            GetHit();
         
        }
    }
}
