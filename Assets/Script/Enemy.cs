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
    internal readonly ChaseEnemyState ChaseEnemyState = new ChaseEnemyState();
    internal AttackEnemyState AttackEnemyState;
    internal readonly DamageEnemyState DamageEnemyState = new DamageEnemyState();
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private bool patrolloop = true;
    [SerializeField] EnemyCharacter character;
    internal EnemyCharacter EnemyCharacter => character;
    [SerializeField] private Transform lookpos1;
    internal Transform Lookpos1 => lookpos1;

    [SerializeField] private Transform lookpos2;
    internal Transform Lookpos2 => lookpos2;

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
        DamageEnemyState.Init(this);
        ChangeState(PatrolState);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
            _currentstate?.UpdateState();
    }

    private void OnDamaged()
    {
        ChangeState(DamageEnemyState);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag($"weapond") && _currentstate != DamageEnemyState)
        {
            OnDamaged();
        }
    }
    internal Vector3 AlarmPos { get; set; }

    internal void OnAlarmed(Vector3 pos)
    {
        AlarmPos = pos;
        if (_currentstate == PatrolState)
        {
            PatrolState.OnAlarmed();
        }
        else if (_currentstate == ChaseEnemyState)
        {
            ChaseEnemyState.OnAlarmed();
        }
    }
}