using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using UnityEngine.UIElements;

public class TaskAttack : Node
{
    private Transform _lastTarget;
    private Animator _animator;
    private EnemyController _enemyController;

    private float _attackCooldown = 1.5f;
    private float _attackCounter = 0f;

    public TaskAttack(Transform transform) 
    {
        _enemyController = transform.GetComponent<EnemyController>();
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (!_enemyController.isDead)
        {
            Transform target = (Transform)GetData("target");
            if (target != _lastTarget)
            {
                //get manager for health biz.
                _lastTarget = target;
            }

            if (_attackCounter <= 0f)
            {
                _animator.Play("Attack");
                _attackCounter = _attackCooldown;
            }
            _attackCounter -= Time.deltaTime;

            state = NodeState.RUNNING;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}