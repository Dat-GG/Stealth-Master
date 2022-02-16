using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class EnemyStates 
{
    protected Enemy Enemy;

    internal void Init(Enemy enemy)
    {
        Enemy = enemy;
    }
    internal abstract void EnterState();

    internal abstract void UpdateState();


    internal abstract void ExitState();
  
}
