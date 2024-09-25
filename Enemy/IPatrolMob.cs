
using BehaviourTree;
using System.Collections.Generic;

public interface IPatrolMob
{
    public List<UnityEngine.Transform> Waypoints { get; set; }
    public EnemyController EnemyController {  get; set; }
    public Node Root {  get; set; }
    public void ResetTarget();
}
