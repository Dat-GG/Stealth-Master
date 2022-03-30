using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Rigidbody[] _rb;

    private void Awake()
    {
        _rb = GetComponentsInChildren<Rigidbody>();
        foreach (var body in _rb)
        {
            body.isKinematic = true;
        }
    }
    internal void AnimateIdle()
    {
        animator.CrossFade("Idle", 0.1f);
    }

    internal void AnimateWalk()
    {
        animator.CrossFade("Walk", 0.1f);
    }

    internal void AnimateRun()
    {
        animator.CrossFade("Run", 0.1f);
    }

    internal void AnimateDie(Vector3 direction)
    {
        animator.enabled = false;
        foreach (var body in _rb)
        {
            body.isKinematic = false;
            body.AddForce(direction * 5000);
        }
    }
}
