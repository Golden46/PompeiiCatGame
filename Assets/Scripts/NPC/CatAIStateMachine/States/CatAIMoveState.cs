using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CatAIMoveState : CatAIBaseState
{
    public CatAIMoveState(CatAIStateMachine currentContext, CatAIStateFactory catAIStateFactory)
    : base (currentContext, catAIStateFactory){}

    private float _timer;
    private bool _animating;
    private PatrolPoint newTarget;

    public override void EnterState(){ MoveToDestination(); }

    public override void UpdateState()
    {
        if (_ctx.InQuestLocation) MoveToDestination();

        if (_ctx.Agent.remainingDistance <= _ctx.StoppingDistance) Animate();
        CheckSwitchStates(); 
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){ }

    private void MoveToDestination()
    {
        _timer = 0.0f;
        _animating = false;

        if (_ctx.InQuestLocation)
        {
            newTarget = _ctx.QuestPoint;
        }
        else
        {
            if (_ctx.PatrolPointsArray.Length == 0) return;

            do newTarget = _ctx.PatrolPointsArray[Random.Range(0, _ctx.PatrolPointsArray.Length)];
            while (newTarget == _ctx.CurrentTarget && _ctx.PatrolPointsArray.Length > 1);
        }

        _ctx.CurrentTarget = newTarget;
        _ctx.Agent.SetDestination(_ctx.CurrentTarget.point.position);
    }

    private void Animate()
    {
        if (!_animating)
        {
            _ctx.Animator.SetBool(_ctx.CurrentTarget.animationTrigger, true);
            _animating = true;
        }

        if (_ctx.InQuestLocation)
        {

        }
        else
        {
            _timer += Time.deltaTime;
            if (_timer >= _ctx.CurrentTarget.animationDuration)
            {
                _ctx.Animator.SetBool(_ctx.CurrentTarget.animationTrigger, false);
                MoveToDestination();
            }
        }
    }
}
