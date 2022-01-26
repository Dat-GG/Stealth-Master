using Funzilla;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private const float WalkSpeed = 2.0f;
    private const float AttackTime = 0.7f;
    private const float AttackRange = 3.0f;
    [SerializeField] private DynamicJoystick Joystick;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Character character;
    private float hp;
    private enum State
    {
        Idle, Walk, Run, Attack, Win, Die
    }
    private Exit exit;
    private Enemy[] enemies;
    private float attackTime;
    private State _state = State.Idle;
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
                    ChangeState(State.Walk);
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
                ChangeState(State.Attack);
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
    private void ExitCurrentState()
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
}
