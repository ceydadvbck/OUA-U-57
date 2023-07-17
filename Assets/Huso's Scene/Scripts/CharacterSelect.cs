using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class CharacterSelect : NetworkBehaviour
{
    [SerializeField] private GameObject characterSelectDisplay = default;
    [SerializeField] private CharacterScriptableObject[] characters = default;

    public int currentCharacterIndex = 0;
    private List<GameObject> characterInstances = new List<GameObject>();
    private bool[] characterSelected;

    GameObject[] skeletonObjects;
    GameObject[] golemObjects;
    List<EnemyController> enemyControllers = new List<EnemyController>();
    List<Golem> golems = new List<Golem>();

    public void Start()
    {
        golemObjects = GameObject.FindGameObjectsWithTag("Golem");
        skeletonObjects = GameObject.FindGameObjectsWithTag("Skeleton");

        foreach (GameObject skeletonObject in skeletonObjects)
        {
            EnemyController enemyController = skeletonObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyControllers.Add(enemyController);
            }
        }
        foreach (GameObject golemObject in golemObjects)
        {
            Golem golem = golemObject.GetComponent<Golem>();
            if (golem != null)
            {
                golems.Add(golem);
            }
        }
    }

    public override void OnStartClient()
    {
        characterSelectDisplay.SetActive(true);
        characterSelected = new bool[characters.Length];
    }


    public void BarbarianButton()
    {
        currentCharacterIndex = 0;
        if (!characterSelected[0])
        {
            CmdSelect(0);
            characterSelected[0] = true;
            characterSelectDisplay.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void RangerButton()
    {
        currentCharacterIndex = 1;
        if (!characterSelected[1])
        {
            CmdSelect(1);
            characterSelected[1] = true;
            characterSelectDisplay.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSelect(int characterIndex, NetworkConnectionToClient sender = null)
    {
        GameObject characterInstance = Instantiate(characters[characterIndex].CharacterPrefab);
        NetworkServer.Spawn(characterInstance, sender);

        foreach (GameObject skeletonObject in skeletonObjects)
        {
            EnemyController enemyController = skeletonObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                GameObject[] updatedTargets = new GameObject[enemyController.target.Length + 1];
                for (int i = 0; i < enemyController.target.Length; i++)
                {
                    updatedTargets[i] = enemyController.target[i];
                }
                updatedTargets[enemyController.target.Length] = characterInstance;

                enemyController.target = updatedTargets;
            }
        }
        foreach (GameObject golemObject in golemObjects)
        {
            Golem golem = golemObject.GetComponent<Golem>();
            if (golem != null)
            {
                GameObject[] updatedTargets2 = new GameObject[golem.target.Length + 1];
                for (int i = 0; i < golem.target.Length; i++)
                {
                    updatedTargets2[i] = golem.target[i];
                }
                updatedTargets2[golem.target.Length] = characterInstance;

                golem.target = updatedTargets2;
            }
        }
    }

}
