using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolPoint : MonoBehaviour
{
    private Vector3 _position;
    void Start()
    {
        _position = transform.position; 
    }
}
