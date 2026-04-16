using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private string itemID;
    [SerializeField] private Transform itemPopup;
    [SerializeField] private bool destroyObject;
    [SerializeField] private string animationTrigger;
    
    private Animator _animator;
    private QuestManager _questManager;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _questManager = QuestManager.Instance;
    }

    public void Pickup()
    {
        var collectItem = _questManager.CheckObjective(itemID); // Check to either complete an objective or pick up objective item
        if (!collectItem) return;
        
        if (destroyObject) Destroy(gameObject); // If either happen then delete the real world object
        if (_animator != null) _animator.SetTrigger(animationTrigger);
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