using UnityEngine;

[CreateAssetMenu(menuName = "AI/AI Action/AI Chase Action")]
public class ChaseAction : ActionBehavior
{
    public override void OnSelected()
    {
        ScoreAction = BEHAVIOR_CHASE;
        bIsInteruptable = true;
        bIsAttackable = false;
        bIsSelected = true;
    }

    public override void simulate()
    {
        baseBehavior.chase();
    }
}
