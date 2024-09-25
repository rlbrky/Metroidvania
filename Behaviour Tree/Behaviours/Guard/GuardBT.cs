using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class GuardBT : BehaviourTree.Tree, IPatrolMob
{
    private List<Transform> _waypoints;
    private EnemyController _enemyController;
    private Node _root;

    public static float speed = 3.0f;
    public static float fovRange = 5.0f;
    public static float attackRange = 2.2f;
    public static int attackDamage = 20;

    public List<Transform> Waypoints { get { return _waypoints; } set { _waypoints = value; } }
    public EnemyController EnemyController { get { return _enemyController; } set { _enemyController = value; } }
    public Node Root { get { return _root; } set { _root = value; } }

    protected override Node SetupTree()
    {
        //Order is important here, this order here is the list of priority.
        //The list we have below has patrolling as its foreback action.
        _root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TaskAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckEnemy(transform),
                new TaskGoToTarget(transform),
            }),
            new Patrol(transform, _waypoints, _enemyController),
        });

        return _root;
    }

    public void ResetTarget()
    {
        _root.GetChild<Patrol>()._currentWaypointIndex = 0;
        _root.SetData("target", null);
    }
}
