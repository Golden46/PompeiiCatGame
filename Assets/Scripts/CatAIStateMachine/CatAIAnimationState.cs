using UnityEngine;

public class CatAIAnimationState : CatAIBaseState
{
    public CatAIAnimationState(CatAIStateMachine currentContext, CatAIStateFactory catAIStateFactory)
    : base (currentContext, catAIStateFactory){}

    private float _timer;

    public override void EnterState() 
    {
        _timer = 0.0f;
        _ctx.Animator.SetTrigger(_ctx.CurrentTarget.animationTrigger); 
    }

    public override void UpdateState() 
    {
        _timer += Time.deltaTime;
        if (_timer >= _ctx.CurrentTarget.animationDuration)
        {
            CheckSwitchStates();
        }
    }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchStates() { SwitchState(_factory.Move()); }
}
