using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillDamage : MonoBehaviour
{
    [SerializeField] int damage;


    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.transform.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterHealth>().TakeDamage(damage);
        }
    }
}
