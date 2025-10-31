using UnityEngine;

public class CatAIStartQuestState : CatAIBaseState
{
    public CatAIStartQuestState(CatAIStateMachine currentContext, CatAIStateFactory catAIStateFactory)
    : base(currentContext, catAIStateFactory) { }

    public override void EnterState() { Object.FindAnyObjectByType<QuestManager>().StartQuest(_ctx.CurrentQuest); }
    public override void UpdateState() { CheckSwitchStates(); }

    public override void ExitState() { }

    public override void CheckSwitchStates() { }
}
