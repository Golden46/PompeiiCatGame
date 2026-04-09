using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Objective")]
public class QuestObjective : ScriptableObject
{
    public string itemID; // Unique ID for item which is used to know if it can be picked up or not based on current Quest.
    public string objectiveDescription; // Just a description for me

    public string requiredItemID; // Nothing if no item is required to be picked up to complete this objective.
}