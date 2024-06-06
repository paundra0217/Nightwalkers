using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI Action/AI Idle Action")]
public class IdleAction : ActionBehavior
{
    public override void OnSelected()
    {
        ScoreAction = BEHAVIOR_IDLE;
        bIsInteruptable = true;
        bIsAttackable = false;
        bIsSelected = true;
    }

    public override void simulate()
    {
        //var wp = baseBehavior.getPatrolWaypoint();
        //baseBehavior.addAction(this);
        //baseBehavior.Patrol();
    }
}