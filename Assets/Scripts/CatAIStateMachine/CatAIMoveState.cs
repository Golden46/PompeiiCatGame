using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CatAIMoveState : CatAIBaseState
{
    public CatAIMoveState(CatAIStateMachine currentContext, CatAIStateFactory catAIStateFactory)
    : base (currentContext, catAIStateFactory){}

    public override void EnterState(){ ChooseNewDestination(); }

    public override void UpdateState()
    {
        Debug.Log("Move");
        CheckSwitchStates(); 
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){ if (_ctx.Agent.remainingDistance <= _ctx.StoppingDistance) SwitchState(_factory.Animation()); }

    private void ChooseNewDestination()
    {
        if (_ctx.PatrolPointsArray.Length == 0) return;

        PatrolPoint newTarget;
        do newTarget = _ctx.PatrolPointsArray[Random.Range(0, _ctx.PatrolPointsArray.Length)];
        while (newTarget == _ctx.CurrentTarget && _ctx.PatrolPointsArray.Length > 1);

        _ctx.CurrentTarget = newTarget;
        _ctx.Agent.SetDestination(_ctx.CurrentTarget.point.position);
    }
}
