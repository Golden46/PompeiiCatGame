using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Objective")]
public class QuestObjective : ScriptableObject
{
    public string objectiveDescription;
    public bool isCompleted;
    public void CompleteObjective()
    {
        isCompleted = true;
        Debug.Log("Objective completed: " + objectiveDescription);
    }
}