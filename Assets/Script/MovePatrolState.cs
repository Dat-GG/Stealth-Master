using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class MovePatrolState : PatrolState
{
    internal override void EnterState()
    {
        _currentState = States.Move;
        Enemy.Agent.SetDestination(_points[0]);
        _reverse = false;
        _targetIndex = 0;
    }

    internal override void ExitState()
    {
        throw new System.NotImplementedException();
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
        //void Start()
        //{
        //    _currentState = States.Move;
        //    Enemy.Agent.SetDestination(_points[0]);
        //    _reverse = false;
        //    _targetIndex = 0;
        //}

        //void Update()
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

        void ChangeStates(States newState)
        {
            if (_currentState == newState) return;
            ExitCurrentState();
            _currentState = newState;
            EnterNewState();
        }

        void EnterNewState()
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

        void ExitCurrentState()
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
                                _targetIndex = _points.Count - 2;
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
}
