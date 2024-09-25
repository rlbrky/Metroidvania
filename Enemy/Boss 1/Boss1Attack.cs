using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Boss1Attack : Node
{
    private Animator _animator;
    private bossHitHandler _hitHandler;

    public static int attackDamage = 20;

    private float _attackCooldown = 0.75f;
    private float _attackCounter = 0f;
    private float _skillCooldown = 3.5f;
    private float _skillCounter = 0f;

    public Boss1Attack(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
        _hitHandler = transform.GetComponentInChildren<bossHitHandler>();
        _hitHandler.attackDamage = attackDamage;
    }

    public override NodeState Evaluate()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (_skillCounter <= 0f)
            {
                int result = Random.Range(0, 2);
                if (result == 0)
                {
                    _animator.Play("Skill4");
                }
                else if (result == 1)
                {
                    _animator.Play("Skill5");
                }
                _skillCounter = _skillCooldown;
            }
            //Max number should be 1 over the number of animations the boss has.
            else if (_attackCounter <= 0f)
            {
                int result = Random.Range(0, 4);
                if (result == 0)
                {
                    _animator.Play("Attack1");
                }
                else if (result == 1)
                {
                    _animator.Play("Skill1");
                }
                else if (result == 2)
                {
                    _animator.Play("Skill2");
                }
                else if (result == 3)
                {
                    _animator.Play("Skill3");
                }
                _attackCounter = _attackCooldown;
            }
            _attackCounter -= Time.deltaTime;
            _skillCounter -= Time.deltaTime;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
