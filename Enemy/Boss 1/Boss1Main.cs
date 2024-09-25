using BehaviourTree;
using System.Collections.Generic;

public class Boss1Main : Tree
{
    public static float speed = 3.0f;
    public static float attackRange = 2.5f;
    
    protected override Node SetupTree()
    {
        Node _root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new Boss1DistanceCheck(transform),
                
            }),
            new Boss1Attack(transform),
        });

        return _root;
    }
}
