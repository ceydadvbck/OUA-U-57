
using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class CharacterHealth : NetworkBehaviour
{

    public Stat maxHealth;
    public int currentHealth;

    public Stat damage;
    public Stat armor;

    int i=0;
  
    [SerializeField] List<GameObject> playerSpawnPoint = new List<GameObject>();

    bool lightAnim;

    void Start()
    {
      
        GameObject[] spawn1 = GameObject.FindGameObjectsWithTag("spawn1"); //0
        GameObject[] spawn2 = GameObject.FindGameObjectsWithTag("spawn2"); //1
        GameObject[] spawn3 = GameObject.FindGameObjectsWithTag("spawn3"); //2
        GameObject[] spawn4 = GameObject.FindGameObjectsWithTag("spawn4"); //3
        playerSpawnPoint.AddRange(spawn1);
        playerSpawnPoint.AddRange(spawn2);
        playerSpawnPoint.AddRange(spawn3);
        playerSpawnPoint.AddRange(spawn4);
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
            i = 0;
        }
        if (other.gameObject.transform.CompareTag("spawn2"))
        {
            i = 1;
        }
        if (other.gameObject.transform.CompareTag("spawn3"))
        {
            i = 2;
        }
        if (other.gameObject.transform.CompareTag("spawn4"))
        {
            i = 3;
        }
        if (other.gameObject.transform.CompareTag("LightAnim") && !lightAnim)
        {
            other.gameObject.GetComponent<Animator>().Play("Light");
            lightAnim = true;
        }

        if (other.gameObject.transform.CompareTag("FinishPoint"))
        {
            //gameObject.GetComponent<AnimationStateController>().enabled = false;

            /*
                oyun sonu timeline

            */
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            CmdRespawnPlayer();
            currentHealth = maxHealth.GetValue();
        }
    }

  
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }



}