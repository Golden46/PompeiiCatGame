using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Settings")] 
    public string questID;
    
    [Header("Quest Information")]
    public string title;
    public string description;
    public CatDialogue catDialogue;
    
    [Header("Quest Objectives")]
    public QuestObjective[] objectives;
}

public enum QuestState
{
    Inactive,
    Active,
    Completed
}
