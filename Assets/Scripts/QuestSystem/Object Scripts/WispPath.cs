using UnityEngine;
using UnityEngine.AI;

public class WispPath : MonoBehaviour
{
    private NavMeshAgent _agent;

    //DEBUG
    [SerializeField] private Transform dest;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _agent.SetDestination(dest.position);
    }   
}
