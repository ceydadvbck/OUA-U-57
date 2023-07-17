using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 500f;
    bool isEnemyDead = false;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Damage(float damage)
    {

        if (!isEnemyDead)
        {
            enemyHealth -= damage;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                EnemyDead();
            }
        }
    }
    public void EnemyDead()
    {
        isEnemyDead = true;
        anim.SetTrigger("BossDeath");
        Destroy(gameObject, 1.3f);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.transform.CompareTag("GroundAttackDamageBox"))
        {
            Damage(50f);

        }
    }
}