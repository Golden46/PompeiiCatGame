using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();
    void Start()
    {
        foreach (var quest in quests)
        {
            foreach (var objective in quest.objectives)
            {
                if (!objective.isCompleted)
                {

                }
            }
        }
    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        Debug.Log("Quest completed: " + quest.title);
    }
    public List<Quest> GetActiveQuests()
    {
        return quests.FindAll(q => !q.isCompleted);
    }
}