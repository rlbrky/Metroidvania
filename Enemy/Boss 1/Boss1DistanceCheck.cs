using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class Boss1DistanceCheck : Node
{
    private Transform _transform;
    private Animator _animator;
    private BossController _bossController;
    private PlayerController _playerController;
    public Boss1DistanceCheck(Transform transform)
    {
        _transform = transform;
        _animator = _transform.GetComponent<Animator>();
        _bossController = _transform.GetComponent<BossController>();
        _playerController = PlayerController.instance;
    }

    public override NodeState Evaluate()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (!_bossController.isColliding && _bossController.shouldFollow && Mathf.Abs(_transform.position.z - _playerController.transform.position.z) > (Boss1Main.attackRange - 1f))
            {
                if (Mathf.Abs(_playerController.transform.position.z - _transform.position.z) > Boss1Main.attackRange)
                {
                    _animator.SetBool("isWalking", true);
                    _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(_transform.position.x, _transform.position.y, _playerController.transform.position.z), Boss1Main.speed * Time.deltaTime);
                }
                else
                {
                    _animator.SetBool("isWalking", false);

                    state = NodeState.FAILURE;
                    return state;
                }

                if (Mathf.Abs(_playerController.transform.position.z - (_transform.forward + _transform.position).z) > Mathf.Abs(_playerController.transform.position.z - _transform.position.z))
                {
                    if (_transform.rotation.y == -180)
                        _transform.rotation = Quaternion.Euler(0, 0, 0);
                    else
                        _transform.rotation *= Quaternion.Euler(0, 180, 0);

                }

                state = NodeState.SUCCESS;
                return state;
            }
            else
                _animator.SetBool("isWalking", false);
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
