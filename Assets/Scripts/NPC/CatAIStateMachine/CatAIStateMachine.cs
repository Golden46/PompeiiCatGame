using UnityEngine;
using UnityEngine.AI;

public class CatAIStateMachine : MonoBehaviour
{
    [SerializeField] private PatrolPoint[] patrolPoints;
    [SerializeField] private PatrolPoint questPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private Color GizmoColor = Color.yellow;
    [SerializeField] private float GizmoRadius = 0.3f;

    private NavMeshAgent _agent;
    private float _stoppingDistance = 0.5f;
    private PatrolPoint _currentTarget;

    private bool _inQuestLocation;

    // state variables
    private CatAIBaseState _currentState;
    private CatAIStateFactory _states;

    // getters and setters
    public CatAIBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PatrolPoint[] PatrolPointsArray { get { return patrolPoints; } }
    public PatrolPoint QuestPoint { get { return questPoint; } }
    public PatrolPoint CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public float StoppingDistance { get { return _stoppingDistance; } }
    public Animator Animator { get { return animator; } }
    public bool InQuestLocation { get { return _inQuestLocation; } set { _inQuestLocation = value; } }

    private void Awake()
    {
        // setup components
        _agent = GetComponent<NavMeshAgent>();

        // setup state
        _states = new CatAIStateFactory(this);
        _currentState = _states.Move();
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    // Debug for patrol points
    private void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Gizmos.color = GizmoColor;

        foreach (PatrolPoint point in patrolPoints)
        {
            if (point != null) Gizmos.DrawSphere(point.point.position, GizmoRadius);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(questPoint.point.position, GizmoRadius);
    }
}

[System.Serializable]
public class PatrolPoint
{
    public Transform point;
    public string animationTrigger;
    public float animationDuration = 2f;
}
