using BehaviourTree;
using UnityEngine;

public class Boss2Attack : Node
{
    private Animator _animator;
    private Boss2Controller _controller;
    private bossHitHandler _hitHandler;

    public static int attackDamage = 20;

    private float _attackCooldown = 0.75f;
    private float _attackCounter = 0f;
    private float _skillCooldown = 3.5f;
    private float _skillCounter = 0f;

    public Boss2Attack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        _controller = transform.GetComponent<Boss2Controller>();
        _hitHandler = transform.GetComponentInChildren<bossHitHandler>();
        _hitHandler.attackDamage = attackDamage;
    }

    public override NodeState Evaluate()
    {
        if (_controller.secondPhase)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (_skillCounter <= 0f)
                {
                    int result = Random.Range(0, 3);
                    if (result == 0)
                    {
                        _animator.Play("SurprizeSpinAttack");
                    }
                    else if (result == 1)
                    {
                        _animator.Play("SpinAttack");
                    }
                    else if (result == 2)
                    {
                        _animator.Play("BigSlam");
                    }
                    _skillCounter = _skillCooldown;
                }
                else if (_attackCounter <= 0f)
                {
                    _animator.Play("Combo1");
                    _attackCounter = _attackCooldown;
                }
                _attackCounter -= Time.deltaTime;
                _skillCounter -= Time.deltaTime;
            }
        }
        else
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (_skillCounter <= 0f)
                {
                    _animator.Play("SurprizeSpinAttack");
                    _skillCounter = _skillCooldown;
                }
                //Max number should be 1 over the number of animations the boss has.
                else if (_attackCounter <= 0f)
                {
                    int result = Random.Range(0, 2);
                    if (result == 0)
                    {
                        _animator.Play("Combo1");
                    }
                    else if (result == 1)
                    {
                        _animator.Play("SurprizeSpinAttack");
                    }
                    _attackCounter = _attackCooldown;
                }
                _attackCounter -= Time.deltaTime;
                _skillCounter -= Time.deltaTime;
            }
        }
       

        state = NodeState.RUNNING;
        return state;
    }

    public void ApplyDebuff() //Needs more work.
    {
        _animator.Play("Debuff");
    }
}
