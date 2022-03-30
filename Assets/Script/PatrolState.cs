using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class PatrolState : EnemyStates
{
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

    internal void OnAlarmed()
    {
        Chase();
    }

    private void Chase()
    {
        Enemy.ChangeState(Enemy.ChaseEnemyState);
    }
}
