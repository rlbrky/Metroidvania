using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class CheckEnemyInAttackRange : Node
{
    private Transform _transform;
    private Animator _animator;

    public CheckEnemyInAttackRange(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object t = GetData("target");
        if(t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Mathf.Abs(_transform.position.z - target.position.z) <= (GuardBT.attackRange + 0.8f))
        {
            _animator.SetBool("isWalking", false);

            state = NodeState.SUCCESS;
            return state;
        }
        //Vector3.Distance(_transform.position, target.position)
        _animator.SetBool("isWalking", true);
        state = NodeState.FAILURE;
        return state;
    }
}
