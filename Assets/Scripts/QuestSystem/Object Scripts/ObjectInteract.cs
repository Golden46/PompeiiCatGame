using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    [SerializeField] private QuestObjective _questObjective;
    [SerializeField] private Quest _quest;

    private QuestManager _questManager;

    private void OnEnable()
    {
        _questManager = QuestManager.Instance;
        if (_quest == _questManager.GetActiveQuest()) PickupItem();
        else enabled = false;
    }

    private void PickupItem()
    {
        _questManager.CompleteObjective(_quest, _questObjective);
        Destroy(gameObject);
    }
}
