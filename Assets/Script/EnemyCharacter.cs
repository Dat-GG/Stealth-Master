using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator;

    internal void AnimateIdle()
    {
        animator.CrossFade("Idle", 0.1f);
    }

    internal void AnimateWalk()
    {
        animator.CrossFade("Walk", 0.1f);
    }
}
