public class CatAIStateFactory 
{
    private CatAIStateMachine _context;

    public CatAIStateFactory(CatAIStateMachine currentContext)
    {
        _context = currentContext;
    }

    public CatAIBaseState Move(){ return new CatAIMoveState(_context, this); }

    public CatAIBaseState StartQuest() { return new CatAIStartQuestState(_context, this); }
}
