using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float health;
    bool characterDead;

    void DamageToCharacter(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            health = 0f;
            characterDead = true;
        }
        else characterDead = false;
    }
}
