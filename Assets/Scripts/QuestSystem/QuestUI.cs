using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    public TextMeshProUGUI questObjText;

    private QuestManager questManager;

    void Start()
    {
        questManager = FindAnyObjectByType<QuestManager>();
    }

    public void UpdateQuestList()
    {
        Quest quest = questManager.GetActiveQuests();
        questTitleText.text = quest.title;
        questDescText.text = quest.description;

        questObjText.text = "";
        foreach (QuestObjective obj in quest.objectives) {
            questObjText.text += $"{obj.objectiveDescription}\n";
        }
    }
}