using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterSelect : NetworkBehaviour
{
    [SerializeField] private GameObject characterSelectDisplay = default;
    [SerializeField] private CharacterScriptableObject[] characters = default;

    private int currentCharacterIndex = 0;
    private List<GameObject> characterInstances = new List<GameObject>();
    private bool[] characterSelected;

    public override void OnStartClient()
    {
        characterSelectDisplay.SetActive(true);
        characterSelected = new bool[characters.Length];
    }


    public void BarbarianButton()
    {
        if (!characterSelected[0])
        {
            CmdSelect(0);
            characterSelected[0] = true;
            characterSelectDisplay.SetActive(false);
        }
    }
    public void RangerButton()
    {
        if (!characterSelected[1])
        {
            CmdSelect(1);
            characterSelected[1] = true;
            characterSelectDisplay.SetActive(false);
        }
    }
    
    [Command(requiresAuthority = false)]
    public void CmdSelect(int characterIndex , NetworkConnectionToClient sender = null)
    {
        GameObject characterInstance = Instantiate(characters[characterIndex].CharacterPrefab);
        NetworkServer.Spawn(characterInstance, sender);
    }
}
