using System;
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
        var completed = _questManager.CompleteObjective(itemID);
        if (completed) Destroy(gameObject);
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