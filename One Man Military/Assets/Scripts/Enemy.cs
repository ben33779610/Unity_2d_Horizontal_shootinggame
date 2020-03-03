using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private float curHP;
	private float HP;

	private void Start()
	{
		HP = 5;
		curHP = HP;
	}
	public void Hit(float damage)
	{
		curHP -= damage;
		if (curHP <= 0) Dead();
	}
	private void Dead()
	{
		
		Destroy(this);
		Destroy(gameObject, 0.5f);
	}
}
