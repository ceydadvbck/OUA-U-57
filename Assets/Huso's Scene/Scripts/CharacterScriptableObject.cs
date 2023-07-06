using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Character Selection/Character")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField] private string characterName = default;
    [SerializeField] private GameObject characterPrefab = default;

    public string CharacterName => characterName;
    public GameObject CharacterPrefab => characterPrefab;
}
