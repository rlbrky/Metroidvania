using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TeleporterRangeCheck : Node
{
    private Transform _transform;
    private Animator _animator;

    public TeleporterRangeCheck(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(_transform.position, target.position) <= TeleporterGuardBT.attackRange)
        {
            _animator.SetBool("isAttacking", true);
            _animator.SetBool("isWalking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
