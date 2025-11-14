using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private RectTransform objectivePanel;
    [SerializeField] private Texture tickedBox;

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    private List<GameObject> questObjects = new List<GameObject>();

    private QuestManager questManager;

    void Start()
    {
        questManager = FindAnyObjectByType<QuestManager>();
    }

    public void EnableQuestUI()
    {
        Quest quest = questManager.GetActiveQuest();
        questTitleText.text = quest.title;
        questDescText.text = quest.description;

        foreach (QuestObjective obj in quest.objectives) {
            GameObject objective = Instantiate(objectivePrefab, objectivePanel.position, Quaternion.identity, objectivePanel);
            questObjects.Add(objective);
            objective.GetComponentInChildren<TextMeshProUGUI>().text = obj.objectiveDescription;
        }
    }

    public void UpdateQuestObjective(QuestObjective objective)
    {
        foreach (GameObject obj in questObjects)
        {
            if (obj.GetComponentInChildren<TextMeshProUGUI>().text == objective.objectiveDescription)
            {
                obj.GetComponentInChildren<RawImage>().texture = tickedBox;
            }
        }
    }
}