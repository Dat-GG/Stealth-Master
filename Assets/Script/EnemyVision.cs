using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
	private Enemy _enemy;
	private void Start()
	{
		_enemy = GetComponentInParent<Enemy>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			_enemy.OnAlarmed(other.transform.position);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			_enemy.AlarmPos = other.transform.position;
			_enemy.Agent.SetDestination(_enemy.AlarmPos);
		}
	}
}
