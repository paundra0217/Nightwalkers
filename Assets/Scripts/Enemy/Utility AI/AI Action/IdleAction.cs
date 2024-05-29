using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI Idle Action")]
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
        baseBehavior = baseBehavior.GetComponent<BaseBehavior>();

        var wp = baseBehavior.getPatrolWaypoint();

        
    }

}
