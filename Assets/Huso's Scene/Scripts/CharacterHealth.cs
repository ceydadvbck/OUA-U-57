
using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CharacterHealth : NetworkBehaviour
{

    public Stat maxHealth;          // Maximum amount of health
    public int currentHealth { get; protected set; }    // Current amount of health

    public Stat damage;
    public Stat armor;

    int i=0;
    public event System.Action OnHealthReachedZero;
    List<GameObject> playerSpawnPoint = new List<GameObject>();

    void Start()
    {
        // "x" tag'ine sahip olan tüm nesneleri bulma
        GameObject[] objectsX = GameObject.FindGameObjectsWithTag("spawn1");
        // "y" tag'ine sahip olan tüm nesneleri bulma
        GameObject[] objectsY = GameObject.FindGameObjectsWithTag("spawn2");
        // "z" tag'ine sahip olan tüm nesneleri bulma
        GameObject[] objectsZ = GameObject.FindGameObjectsWithTag("spawn3");

        GameObject[] objectsA = GameObject.FindGameObjectsWithTag("spawn4");
        playerSpawnPoint.AddRange(objectsX);
        playerSpawnPoint.AddRange(objectsY);
        playerSpawnPoint.AddRange(objectsZ);
        playerSpawnPoint.AddRange(objectsA);
    }


    public virtual void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!hasAuthority)
            return;

        if (other.gameObject.transform.CompareTag("Trap"))
        {
            CmdRespawnPlayer();
        }

        if (other.gameObject.transform.CompareTag("spawn1"))
        {
            playerSpawnPoint[0] = other.gameObject;
            i = 0;
        }
        if (other.gameObject.transform.CompareTag("spawn2"))
        {
            playerSpawnPoint[1] = other.gameObject;
            i = 1;
        }
        if (other.gameObject.transform.CompareTag("spawn3"))
        {
            playerSpawnPoint[2] = other.gameObject;
            i = 2;
        }
        if (other.gameObject.transform.CompareTag("spawn4"))
        {
            playerSpawnPoint[3] = other.gameObject;
            i = 3;
        }
    }
    [Command]
    private void CmdRespawnPlayer()
    {
        RpcRespawnPlayer();
    }

    [ClientRpc]
    private void RpcRespawnPlayer()
    {
        gameObject.transform.position = playerSpawnPoint[i].transform.position;
    }

    // Damage the character
    public void TakeDamage(int damage)
    {
        // Subtract the armor value - Make sure damage doesn't go below 0.
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        // Subtract damage from health
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        // If we hit 0. Die.
        if (currentHealth <= 0)
        {
            if (OnHealthReachedZero != null)
            {
                OnHealthReachedZero();
            }
        }
    }

    // Heal the character.
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }



}