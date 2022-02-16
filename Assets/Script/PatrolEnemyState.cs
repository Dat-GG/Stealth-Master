using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

internal class PatrolEnemyState : EnemyStates
{
   private enum States
    {
        Move,
        Turn
    }

    private States _currentState;
    private List<Vector3> _points;
    private bool _loop;
    private bool _reverse;
    private int _targetIndex;

    internal void Init(List<Vector3> points, bool loop) 
    {
        _points = points;
        _loop = loop;
    }

    internal override void EnterState()
    {
        _currentState = States.Move;
        Enemy.Agent.SetDestination(_points[0]);
        _reverse = false;
        _targetIndex = 0;
    }

    internal override void UpdateState()
    {
        switch (_currentState)
        {
            case States.Move:
                var a = Vector3.Distance(Enemy.transform.position, _points[_targetIndex]);
                if (a <= 0.1f)
                {
                    ChangeStates(States.Turn);
                }
                break;
            case States.Turn:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    internal override void ExitState()
    {
       
    }

    private void ChangeStates(States newState)
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
            case States.Move:
                Enemy.Agent.SetDestination(_points[_targetIndex]);
                break;
            case States.Turn:
                Enemy.transform.DOLookAt(_points[_targetIndex], 1f).OnComplete
                    (() => { ChangeStates(States.Move); });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ExitCurrentState()
    {
        switch (_currentState)
        {
            case States.Move:
                if (_reverse)
                {
                    _targetIndex--;
                    if (_targetIndex < 0)
                    {
                        _reverse = false;
                        _targetIndex = 1;
                    }
                }
                else
                {
                    _targetIndex++;
                    if (_targetIndex >= _points.Count)
                    {
                        if (_loop)
                        {
                            _targetIndex = 0;
                        }
                        else
                        {
                            _targetIndex = _points.Count -2;
                            _reverse = true;
                        }
                    }
                }
                break;
            case States.Turn:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
