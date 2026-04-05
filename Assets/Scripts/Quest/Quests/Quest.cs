using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Information")]
    public string title;
    public string description;
    public bool isCompleted;
    public QuestObjective[] objectives;
}
