using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private string itemID;
    private QuestManager _questManager;
    
    private void Start()
    {
        _questManager = QuestManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var completed = _questManager.CompleteObjective(itemID);
        if (completed) Destroy(gameObject);
    }
}
