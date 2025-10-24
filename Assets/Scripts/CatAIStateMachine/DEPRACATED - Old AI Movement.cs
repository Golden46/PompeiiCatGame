using UnityEngine;
using UnityEngine.AI;
/*
public class CatAIMovement : MonoBehaviour
{
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] private Color GizmoColor = Color.yellow;
    [SerializeField] private float GizmoRadius = 0.3f;

    private NavMeshAgent _agent;
    private float _stoppingDistance = 0.5f;
    private Transform _currentTarget;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ChooseNewDestination();
    }

    void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _stoppingDistance)
        {
            ChooseNewDestination();
        }

    }

    private void ChooseNewDestination()
    {
        if (PatrolPoints.Length == 0) return;

        Transform newTarget;
        do newTarget = PatrolPoints[Random.Range(0, PatrolPoints.Length)];
        while (newTarget == _currentTarget && PatrolPoints.Length > 1);

        _currentTarget = newTarget;
        _agent.SetDestination(_currentTarget.position);
    }

    private void OnDrawGizmos()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0) return;

        Gizmos.color = GizmoColor;

        foreach (Transform point in PatrolPoints)
        {
            if (point != null) Gizmos.DrawSphere(point.position, GizmoRadius);
        }
    }
}
*/