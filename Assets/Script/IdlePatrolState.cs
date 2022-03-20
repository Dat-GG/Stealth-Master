using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

internal class IdlePatrolState : PatrolState
{
    [SerializeField] private float rotateSpeed = 50;
    private enum States
    {
        Idle,
        Rotate
    }

    private States _currentState;

    // Idle
    private Vector3 _lookingPoint0;
    private Vector3 _lookingPoint1;
    private Vector3 _startPosition;

    internal void Init(Enemy enemy, Transform idleDirection)
    {
        Init(enemy);
        var e = Enemy.transform;
        _startPosition = e.position;
        _lookingPoint0 = _startPosition + e.forward;
        _lookingPoint1 = idleDirection.position;
        _currentState = States.Rotate;
    }
    internal override void EnterState()
    {
        _currentState = States.Idle;
        Enemy.Agent.SetDestination(_startPosition);
    }

    internal override void UpdateState()
    {
        switch (_currentState)
        {
            case States.Idle:
                {
                    var d = Vector3.Distance(Enemy.transform.position, _startPosition);
                    if (d <= 0.1f)
                    {
                        ChangeState(States.Rotate);
                    }
                }

                break;
            case States.Rotate:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void ChangeState(States newState)
    {
        if (_currentState == newState) return;
        ExitCurrentState();
        _currentState = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (_currentState)
        {
            case States.Idle:
                Enemy.Agent.SetDestination(_startPosition);
                break;
            case States.Rotate:
                {
                    Rotate(_lookingPoint0, rotateSpeed, () =>
                    {
                        Rotate(_lookingPoint1, rotateSpeed, null).SetLoops(-1, LoopType.Yoyo);
                    });
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ExitCurrentState()
    {
        switch (_currentState)
        {
            case States.Idle:
                break;
            case States.Rotate:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
