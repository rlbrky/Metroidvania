using UnityEngine;

using BehaviourTree;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

public class Patrol : Node
{
    private Transform _transform;
    private Animator _animator;
    private Vector3[] _waypoints;
    private EnemyController _enemyController;

    private float _waitTime = 1f; //in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;
    private Vector3 wp;

    public int _currentWaypointIndex;
    public Patrol(Transform transform, List<Transform> waypoints, EnemyController enemyController)
    {
        _transform = transform;
        _animator = transform.GetComponent<Animator>();
        _enemyController = enemyController;

        if (_waypoints == null && waypoints != null)
        {
            int i = 0;
            _waypoints = new Vector3[waypoints.Count];
            foreach(var waypoint in waypoints)
            {
                _waypoints[i] = waypoint.position;
                i++;
            }
        }
        else if(_enemyController.isDead)
        {
            _waypoints = null;
        }
    }

    public override NodeState Evaluate()
    {
        if (_enemyController.isColliding && !_enemyController.routeChanged)
        {
            _waitCounter = 0f;
            _waiting = true;

            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            wp = _waypoints[_currentWaypointIndex];
            _animator.SetBool("isWalking", false);

            _enemyController.routeChanged = true;
        }

        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                _animator.SetBool("isWalking", true);

                if (Mathf.Abs(wp.z - (_transform.forward + _transform.position).z) > Mathf.Abs(wp.z - _transform.position.z))
                    _transform.rotation *= Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
        }
        else
        {
            wp = _waypoints[_currentWaypointIndex];
            //if ((Vector3.Distance(_transform.position, wp.position) < 0.01f))
            if (Mathf.Abs(_transform.position.z - wp.z) < 0.01f)
            {
                //_transform.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _animator.SetBool("isWalking", false);

            }
            else
            {
                _animator.SetBool("isWalking", true);
                _transform.position = Vector3.MoveTowards(_transform.position, new Vector3(_transform.position.x, _transform.position.y, wp.z),  GuardBT.speed * Time.deltaTime);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
