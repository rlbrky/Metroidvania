using BehaviourTree;
using UnityEngine;

public class GhoulCheckEnemy : Node
{
    private Transform _transform;
    private Animator _animator;

    public GhoulCheckEnemy(Transform transform)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        object target = GetData("target");
        if (target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position + (_transform.forward * GhoulGuardBT.fovRange), GhoulGuardBT.fovRange, 10);
            if(colliders.Length > 0 )
            {
                parent.parent.SetData("target", colliders[0].transform);
                _animator.Play("Scream");
                parent.GetChild<GhoulGoToTarget>().timer = 0f;

                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.FAILURE;
            return state;
        }
        //_animator.SetBool("isWalking", true);
        state = NodeState.SUCCESS;
        return state;
    }
}
