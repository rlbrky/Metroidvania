using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private Animator _animator;
    private EnemyController _enemyController;

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
        _animator = _transform.GetComponent<Animator>();
        _enemyController = _transform.GetComponent<EnemyController>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(!_enemyController.isColliding && _enemyController.shouldFollow && Mathf.Abs(_transform.position.z - target.position.z) > 0.01f)
        {
            if (Mathf.Abs(target.position.z - _transform.position.z) > GhoulGuardBT.attackRange)
            {
                _animator.SetBool("isWalking", true);
                _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(_transform.position.x, _transform.position.y, target.position.z), GhoulGuardBT.speed * Time.deltaTime);
            }
            else
                _animator.SetBool("isWalking", false);

            if (Mathf.Abs(target.position.z - (_transform.forward + _transform.position).z) > Mathf.Abs(target.position.z - _transform.position.z))
            {
                if (_transform.rotation.y == -180)
                    _transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    _transform.rotation *= Quaternion.Euler(0, 180, 0);

            }
        }
        else
            _animator.SetBool("isWalking", false);

        state = NodeState.RUNNING;
        return state;
    }
}
