using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class DamageEnemyState : EnemyStates
{
    internal override void EnterState()
    {
        Enemy.EnemyCharacter.AnimateDie(PlayerController.Instance.transform.forward);
    }

    internal override void ExitState()
    {
        
    }

    internal override void UpdateState()
    {
       
    }
}
