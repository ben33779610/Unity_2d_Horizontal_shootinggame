using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damage;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "enemy")
		{
			other.GetComponent<Enemy>().Hit(damage);
			Destroy(gameObject);
		}


	}
}
