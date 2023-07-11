using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public class CharacterCombat : MonoBehaviour
{

	public float attackSpeed = 1f;
	private float attackCooldown = 0f;

	public float attackDelay = .6f;

	public event System.Action OnAttack;

	CharacterHealth myStats;

	void Start()
	{
		myStats = GetComponent<CharacterHealth>();
	}

	void Update()
	{
		attackCooldown -= Time.deltaTime;
	}

	//public void Attack(CharacterHealth targetStats)
	//{
	//	if (attackCooldown <= 0f)
	//	{
	//		StartCoroutine(DoDamage(targetStats, attackDelay));

	//		if (OnAttack != null)
	//			OnAttack();

	//		attackCooldown = 1f / attackSpeed;
	//	}

	//}

	IEnumerator DoDamage(CharacterHealth stats, float delay)
	{
		yield return new WaitForSeconds(delay);

		//stats.TakeDamage(myStats.damage.GetValue());
	}

}