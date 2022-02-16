using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

internal class Enemy : MonoBehaviour
{
    private EnemyStates _currentstate;
    internal PatrolEnemyState PatrolEnemyState = new PatrolEnemyState();
    internal ChaseEnemyState ChaseEnemyState = new ChaseEnemyState();
    internal AttackEnemyState AttackEnemyState;
    internal DamageEnemyState DamageEnemyState = new DamageEnemyState();
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool patrolloop = true;

    internal NavMeshAgent Agent => agent;
    protected virtual void Start()
    {
        InitAttackEnemyState();
        var points = new List<Vector3>();
        var enemypatrolpoints = GetComponentsInChildren<EnemyPatrolPoint>();
        points.Add(transform.position);
        foreach (var p in enemypatrolpoints)
        {
            points.Add(p.transform.position);
        }
        PatrolEnemyState.Init(points, patrolloop);
        PatrolEnemyState.Init(this);
        ChangeState(PatrolEnemyState);
    }

    // Update is called once per frame
    private void Update()
    {
            _currentstate?.UpdateState();
    }
    internal void ChangeState(EnemyStates newState)
    {
        if (_currentstate == newState) return;
        _currentstate?.ExitState();
        _currentstate = newState;
        _currentstate?.EnterState();
    }

    protected void InitAttackEnemyState()
    {

    }
}
