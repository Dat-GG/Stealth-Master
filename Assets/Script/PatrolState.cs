using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class PatrolState : EnemyStates
{
    //internal enum States
    //{
    //    Move,
    //    Turn,
    //    Idle
    //}

    //internal States _currentState;
    //internal List<Vector3> _points;
    //internal bool _loop;
    //internal bool _reverse;
    //internal int _targetIndex;
    //[SerializeField] EnemyCharacter EnemyCharacter;

    //internal void Init(List<Vector3> points, bool loop)
    //{
    //    _points = points;
    //    _loop = loop;
    //}

    //internal override void EnterState()
    //{
    //    _currentState = States.Move;
    //    Enemy.Agent.SetDestination(_points[0]);
    //    _reverse = false;
    //    _targetIndex = 0;
    //    throw new ArgumentOutOfRangeException();
    //}

    //internal override void UpdateState()
    //{
    //    switch (_currentState)
    //    {
    //        case States.Move:
    //            var a = Vector3.Distance(Enemy.transform.position, _points[_targetIndex]);
    //            if (a <= 0.1f)
    //            {
    //                ChangeStates(States.Turn);
    //            }
    //            break;
    //        case States.Turn:
    //            break;
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    protected Tweener Rotate(Vector3 lookAt, float turnSpeed, Action onComplete)
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

    internal override void ExitState()
    {
        
    }

    //private void ChangeStates(States newState)
    //{
    //    if (_currentState == newState) return;
    //    ExitCurrentState();
    //    _currentState = newState;
    //    EnterNewState();
    //}

    //private void EnterNewState()
    //{
    //    switch (_currentState)
    //    {
    //        case States.Move:
    //            Enemy.Agent.SetDestination(_points[_targetIndex]);
    //            EnemyCharacter.AnimateWalk();
    //            break;
    //        case States.Turn:
    //            Enemy.transform.DOLookAt(_points[_targetIndex], 1f).OnComplete
    //                (() => { ChangeStates(States.Move); });
    //            break;
    //    }
    //}

    //private void ExitCurrentState()
    //{
    //    switch (_currentState)
    //    {
    //        case States.Move:
    //            if (_reverse)
    //            {
    //                _targetIndex--;
    //                if (_targetIndex < 0)
    //                {
    //                    _reverse = false;
    //                    _targetIndex = 1;
    //                }
    //            }
    //            else
    //            {
    //                _targetIndex++;
    //                if (_targetIndex >= _points.Count)
    //                {
    //                    if (_loop)
    //                    {
    //                        _targetIndex = 0;
    //                    }
    //                    else
    //                    {
    //                        _targetIndex = _points.Count - 2;
    //                        _reverse = true;
    //                    }
    //                }
    //            }
    //            break;
    //        case States.Turn:
    //            break;
    //    }
    //}
}
