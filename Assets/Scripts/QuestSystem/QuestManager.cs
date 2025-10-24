using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestUI questUI;

    public Quest activeQuest;
    public bool isActive;

    private void Start()
    {
        questUI = GetComponent<QuestUI>();
    }

    public void StartQuest(Quest quest)
    {
        if (isActive) return;

        activeQuest = quest;
        isActive = true;
        questUI.UpdateQuestList();
    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        isActive = false;
        Debug.Log("Quest completed: " + quest.title);
    }

    public Quest GetActiveQuests()
    {
        return activeQuest;
    }
}