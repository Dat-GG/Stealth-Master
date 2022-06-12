using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
	private Enemy enemy;
	private void Start()
	{
		enemy = GetComponentInParent<Enemy>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			enemy.OnAlarmed(other.transform.position);
			Debug.LogError("1");
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			enemy.AlarmPos = other.transform.position;
			enemy.Agent.SetDestination(enemy.AlarmPos);
		}
	}
}
