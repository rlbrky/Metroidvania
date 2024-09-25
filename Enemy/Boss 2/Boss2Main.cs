using BehaviourTree;
using System.Collections.Generic;
public class Boss2Main : BehaviourTree.Tree
{
    public static float speed = 6.0f;
    public static float attackRange = 4.5f;
    Node _root;
    protected override Node SetupTree()
    {
         _root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Boss2CheckDistance(transform),
            }),
            new Boss2Attack(transform),
        });

        return _root;
    }
}
