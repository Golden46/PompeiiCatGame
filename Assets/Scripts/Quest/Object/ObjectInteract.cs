using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private string itemID;
    [SerializeField] private Transform itemPopup;
    private QuestManager _questManager;
    
    private void Start()
    {
        _questManager = QuestManager.Instance;
    }

    public void Pickup()
    {
        var collectItem = _questManager.CheckObjective(itemID); // Check to either complete an objective or pick up objective item
        if (collectItem) Destroy(gameObject); // If either happen then delete the real world object
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        itemPopup.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        itemPopup.gameObject.SetActive(false);
    }
}