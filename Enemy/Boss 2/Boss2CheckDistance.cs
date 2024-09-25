using BehaviourTree;
using UnityEngine;

public class Boss2CheckDistance : Node
{
    private Transform _transform;
    private Animator _animator;
    private Boss2Controller _bossController;
    private PlayerController _playerController;

    private float debuffCD = 10f;
    private float debuffCDcounter = 0;

    public Boss2CheckDistance(Transform transform)
    {
        _transform = transform;
        _animator = _transform.GetComponent<Animator>();
        _bossController = _transform.GetComponent<Boss2Controller>();
        _playerController = PlayerController.instance;
    }

    public override NodeState Evaluate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (!_bossController.isColliding && _bossController.shouldFollow && Mathf.Abs(_transform.position.z - _playerController.transform.position.z) > (Boss2Main.attackRange - 1f))
            {
                 if (Mathf.Abs(_playerController.transform.position.z - _transform.position.z) > Boss2Main.attackRange)
                 {
                    if (debuffCDcounter < 0)
                    {
                        parent.parent.GetChild<Boss2Attack>().ApplyDebuff();
                        debuffCDcounter = debuffCD;

                        //Be careful here.
                        state = NodeState.SUCCESS;
                        return state;
                    }
                    else
                    {
                        _animator.SetBool("isWalking", true);
                        _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(_transform.position.x, _transform.position.y, _playerController.transform.position.z), Boss2Main.speed * Time.deltaTime);
                    }
                 }
                 else
                 {
                    _animator.Play("Idle");
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
        debuffCDcounter -= Time.deltaTime;

        state = NodeState.FAILURE;
        return state;
    }
}
