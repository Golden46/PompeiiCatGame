using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public QuestUI questUI;

    public Quest activeQuest;
    public bool isActive;
    private float _completedObjs;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        questUI = GetComponent<QuestUI>();
    }

    public void StartQuest(Quest quest)
    {
        if (isActive) return;

        activeQuest = quest;
        _completedObjs = 0;
        isActive = true;
        questUI.EnableQuestUI();
    }

    public void CompleteObjective(Quest quest, QuestObjective objective)
    {
        foreach (QuestObjective obj in quest.objectives)
        {
            if (objective == obj)
            {
                _completedObjs++;
                obj.CompleteObjective();
                questUI.UpdateQuestObjective(obj);
            }
        }

        if (_completedObjs >= quest.objectives.Length) CompleteQuest(quest);
    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        Debug.Log("Quest completed: " + quest.title);
    }

    public void FinishQuest()
    {
        isActive = false;
        activeQuest = null;
        _completedObjs = 0;
        questUI.DisableQuestUI();
    }

    public Quest GetActiveQuest()
    {
        return activeQuest;
    }
}