using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private string itemID;
    [SerializeField] private Transform itemPopup;
    [SerializeField] private bool destroyObject;
    [SerializeField] private string animationTrigger;

    private bool _collected;
    
    private Animator _animator;
    private QuestManager _questManager;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _questManager = QuestManager.Instance;
    }

    public void Pickup()
    {
        if (_collected) return;
        
        var collectItem = _questManager.CheckObjective(itemID); // Check to either complete an objective or pick up objective item
        
        // If the item can't be collected yet
        if (!collectItem)
        {
            // Send subtitle feedback
            SubtitleManager.Instance.ShowSubtitle(_questManager.itemCollectErrorMessages[Random.Range(0, _questManager.itemCollectErrorMessages.Length)]);
            return;   
        }
        
        // If the object is collected then send subtitle feedback and either animate it or destroy it
        SubtitleManager.Instance.ShowSubtitle($"{_questManager.itemCollectMessages[Random.Range(0, _questManager.itemCollectMessages.Length)]} {itemID.Replace("_item", "")}.");
        _collected = true;  
        if (destroyObject) Destroy(gameObject); 
        if (_animator != null) _animator.SetTrigger(animationTrigger);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || _collected) return;
        itemPopup.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || _collected) return;
        itemPopup.gameObject.SetActive(false);
    }
}