using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ChaseEnemyState : EnemyStates
{
    private enum States
    {
        Run,
        Scout
    }

    private States _state;
    private float RotateSpeed = 90.0f;
    private float LookingTime = 3.0f;
    private float _lookingTime;
    internal override void EnterState()
    {
        OnAlarmed();
    }

    internal override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    internal override void UpdateState()
    {
        switch (_state)
        {
            case States.Run:
                var d = Vector3.Distance(Enemy.transform.position, Enemy.AlarmPos);
                if (d <= 0.1f)
                {
                    ChangeState(States.Scout);
                }
                break;
            case States.Scout:
                _lookingTime -= Time.deltaTime;
                if (_lookingTime <= 0)
                {
                    Enemy.ChangeState(Enemy.PatrolState);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ExitCurrentState()
    {
        switch (_state)
        {
            case States.Run:
                break;
            case States.Scout:
                break;
        }
    }

    private void EnterNewState()
    {
        switch (_state)
        {
            case States.Run:
                Enemy.Agent.SetDestination(Enemy.AlarmPos);
                Enemy.EnemyCharacter.AnimateRun();
                break;
            case States.Scout:
                Enemy.EnemyCharacter.AnimateIdle();
                Rotate(Enemy.Lookpos1.position, RotateSpeed, () =>
                {
                    Rotate(Enemy.Lookpos2.position, RotateSpeed, null).SetLoops(-1, LoopType.Yoyo);
                });
                _lookingTime = LookingTime;
                break;
        }
    }

    private void ChangeState(States newState)
    {
        if (_state == newState) return;
        ExitCurrentState();
        _state = newState;
        EnterNewState();
    }

    internal void OnAlarmed()
    {
        _state = States.Scout;
        ChangeState(States.Run);
    }

    internal Tweener Rotate(Vector3 lookAt, float turnSpeed, Action onComplete)
    {
        var transform = Enemy.transform;
        var v1 = transform.forward;
        var v2 = (lookAt - transform.position).normalized;
        var angle = Vector3.Angle(v1, v2);
        return Enemy.transform.DOLookAt(lookAt, angle / turnSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
