using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TeleportingAttack : Node
{
    private Transform _lastTarget;
    //Get some sort of manager here for health removal biz.
    private Transform _transform;

    private float _attackCooldown = 1f;
    private float _attackCounter = 0f;

    private float offset = 2f; //Teleport distance offset.

    public TeleportingAttack(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target != _lastTarget)
        {
            //get manager for health biz.
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter > _attackCooldown)
        {
            //health removal biz.
            if(_lastTarget.transform.rotation.y < 180.0f)
                _transform.position = _lastTarget.transform.position - new Vector3(0.0f, 0.0f, 1.0f) * offset;
            else
                _transform.position = _lastTarget.transform.position - new Vector3(0.0f, 0.0f, -1.0f) * offset;

            _attackCounter = 0f;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
