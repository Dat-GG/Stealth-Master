using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyStates _currentstate;
    void Start()
    {
        _currentstate = GetInitState();
        if (_currentstate != null)
            _currentstate.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentstate != null)
            _currentstate.UpdateState();
    }
    protected virtual EnemyStates GetInitState()
    {
        return null;
    }
    public void ChangeState(EnemyStates newState)
    {
        _currentstate.ExitState();
        _currentstate = newState;
        _currentstate.EnterState();
    }
}
