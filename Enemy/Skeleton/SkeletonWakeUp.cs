using UnityEngine;

using BehaviourTree;

public class SkeletonWakeUp : Node
{
    private Transform _transform;
    private Animator _animator;

    public float _waitCounter = 0f;

    public bool _isAnimPlaying;
    public bool _waiting = true;

    public SkeletonWakeUp(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            if(!_isAnimPlaying)
            {
                _animator.Play("WakeUp");
                _isAnimPlaying = true;
            }
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp") && (_waitCounter < _animator.GetCurrentAnimatorStateInfo(0).length))
                _waitCounter += Time.deltaTime;
            else if (_waitCounter > _animator.GetCurrentAnimatorStateInfo(0).length)
                _waiting = false;
        }
        else
        {
            state = NodeState.SUCCESS;
            return state;
        }
        state = NodeState.RUNNING;
        return state;
    }
}
