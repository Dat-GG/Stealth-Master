using Funzilla;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private const float WalkSpeed = 9.99f;
    private const float AttackTime = 0.7f;
    private const float AttackRange = 4.0f;
    [SerializeField] private DynamicJoystick Joystick;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Character character;
    private float hp;

    internal static PlayerController Instance { get; private set; }
    private enum State
    {
        Idle, Walk, Run, Attack, Win, Die
    }
    private Exit exit;
    private Enemy[] enemies;
    private Enemy _target;
    private float attackTime;
    private State _state = State.Idle;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        enemies = FindObjectsOfType<Enemy>();
        exit = FindObjectOfType<Exit>();
        hp = 50;

    }

    private void Update()
    {
        var v = Joystick.Direction * 10;
        agent.velocity = new Vector3(v.x, 0, v.y);
        var a = agent.velocity.magnitude;
        if (_state != State.Die && Vector3.Distance(transform.position, exit.transform.position) < 1.0f)
        {
            ChangeState(State.Win);
            return;
        }
        if (hp <= 0)
        {
            ChangeState(State.Die);
        }
        switch (_state)
        {
            case State.Idle:
                if (TryAttack()) return;
                if (a > 0) ChangeState(State.Walk);
                break;
            case State.Walk:
                if (TryAttack()) return;
                if (a > WalkSpeed) ChangeState(State.Run);
                else if (a <= 0) ChangeState(State.Idle);
                break;
            case State.Run:
                if (TryAttack()) return;
                if (a <= WalkSpeed) ChangeState(State.Walk);
                break;
            case State.Attack:
                attackTime -= Time.deltaTime;
                if (attackTime <= 0)
                {
                    ChangeState(State.Walk);
                }
                else
                {
                    agent.velocity = Vector3.zero;
                    var dir = _target.transform.position - transform.position;
                    transform.forward = dir;
                    transform.position += dir.normalized * Time.smoothDeltaTime * 2;                    
                }
                    
                break;
            case State.Win:
                break;
            case State.Die:
                break;

        }
    }
    private bool TryAttack()
    {
        foreach (var e in enemies)
        {
            if (Vector3.Distance(e.transform.position, transform.position) < AttackRange)
            {
                _target = e;
                //ChangeState(State.Attack);
                return true;
            }
        }
        return false;
    }
    private void ChangeState(State newstate)
    {
        if (newstate == _state) return;
        ExitCurrentState();
        _state = newstate;
        EnterNewState();
    }
    private void EnterNewState()
    {
        switch (_state)
        {
            case State.Idle:
                character.AnimateIdle();
                break;
            case State.Walk:
                character.AnimateWalk();
                break;
            case State.Run:
                character.AnimateRun();
                break;
            case State.Attack:
                character.AnimateAttack();
                attackTime = AttackTime;
                break;
            case State.Win:
                break;
            case State.Die:
                break;

        }
    }
    private void ExitCurrentState()
    {
        switch (_state)
        {
            case State.Idle:                
                break;
            case State.Walk:                
                break;
            case State.Run:                
                break;
            case State.Attack:              
                break;
            case State.Win:
                break;
            case State.Die:
                break;

        }
    }
}
