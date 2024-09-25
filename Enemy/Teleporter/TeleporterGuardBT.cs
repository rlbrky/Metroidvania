using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class TeleporterGuardBT : BehaviourTree.Tree, IPatrolMob
{
    private List<Transform> _waypoints;
    private EnemyController _enemyController;
    private Node _root;

    public static float speed = 3.0f;
    public static float fovRange = 5.0f;
    public static float attackRange = 2.5f;
    public static int attackDamage = 20;

    public List<Transform> Waypoints { get { return _waypoints; } set { _waypoints = value; } }
    public EnemyController EnemyController { get { return _enemyController; } set { _enemyController = value; } }
    public Node Root { get { return _root; } set { _root = value; } }

    protected override Node SetupTree()
    {
        //Order is important here, this order here is the list of priority.
        //The list we have below has patrolling as its foreback action.
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyInAttackRange(transform),
                new TeleportingAttack(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckEnemy(transform),
                new TaskGoToTarget(transform),
            }),
            new Patrol(transform, _waypoints, _enemyController),
        });

        return root;
    }

    public void ResetTarget()
    {
        throw new System.NotImplementedException();
    }
}
