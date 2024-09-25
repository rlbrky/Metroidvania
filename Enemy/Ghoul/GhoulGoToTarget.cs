using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using System.Threading;

public class GhoulGoToTarget : Node
{
    private Transform _transform;
    private Animator _animator;
    private EnemyController _enemyController;

    public float timer; //There is a timer to wait for ghoul's scream.

    public GhoulGoToTarget(Transform transform)
    {
        _transform = transform;
        _animator = _transform.GetComponent<Animator>();
        _enemyController = _transform.GetComponent<EnemyController>();
    }

    public override NodeState Evaluate()
    {
        if(!_enemyController.isDead)
        {
            Transform target = (Transform)GetData("target");
            //if(Vector3.Distance(_transform.position, target.position) > 0.01f)
            if (!_enemyController.isColliding && _enemyController.shouldFollow && Mathf.Abs(_transform.position.z - target.position.z) > 0.01f)
            {
                if (timer > 1.66f && !_enemyController.healthManager._isKnockingBack && !_enemyController._dashActive)
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
                    timer += Time.deltaTime;
            }
            else
                _animator.SetBool("isWalking", false);

            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
