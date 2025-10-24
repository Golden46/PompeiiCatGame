using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;

    private QuestManager questManager;

    void Start()
    {
        questManager = FindAnyObjectByType<QuestManager>();
        UpdateQuestList();
    }

    void UpdateQuestList()
    {
        foreach (Quest quest in questManager.GetActiveQuests())
        {
            questTitleText.text = quest.title;
            questDescText.text = quest.description;
        }
    }
}