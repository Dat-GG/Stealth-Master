using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

internal class Enemy : MonoBehaviour
{
    private EnemyStates _currentstate;
    internal PatrolState PatrolState;
    [SerializeField] private PatrolType patrolType;
    [SerializeField] private Transform LookingDir;
    internal ChaseEnemyState ChaseEnemyState = new ChaseEnemyState();
    internal AttackEnemyState AttackEnemyState;
    internal DamageEnemyState DamageEnemyState = new DamageEnemyState();
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool patrolloop = true;
    [SerializeField] EnemyCharacter character;
    internal EnemyCharacter EnemyCharacter => character;

    enum PatrolType
    {
        Idle, Walk
    }

    internal NavMeshAgent Agent => agent;
    protected virtual void Start()
    {
        InitAttackEnemyState();
        if (patrolType == PatrolType.Walk)
        {
            var movePatrol = new MovePatrolState();
            var points = new List<Vector3>();
            var enemypatrolpoints = GetComponentsInChildren<EnemyPatrolPoint>();
            points.Add(transform.position);
            foreach (var p in enemypatrolpoints)
            {
                points.Add(p.transform.position);
            }
            movePatrol.Init(this, points, patrolloop);
            PatrolState = movePatrol;
        }
        else
        {
            var idlePatrol = new IdlePatrolState();
            idlePatrol.Init(this, LookingDir);
            PatrolState = idlePatrol;
        }
        
        ChangeState(PatrolState);
    }

    // Update is called once per frame
    protected virtual void Update()
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
