using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private Transform objectivePanel;
    [SerializeField] private Sprite tickedBox;

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    private readonly List<GameObject> _questObjects = new();

    private QuestManager _questManager;

    private void Start()
    {
        _questManager = FindAnyObjectByType<QuestManager>();
    }

    public void EnableQuestUI()
    {
        questPanel.SetActive(true);
        var quest = _questManager.GetActiveQuest();
        questTitleText.text = quest.title;
        questDescText.text = quest.description;

        foreach (QuestObjective obj in quest.objectives) {
            var objective = Instantiate(objectivePrefab, objectivePanel.position, Quaternion.identity, objectivePanel);
            _questObjects.Add(objective);
            objective.GetComponent<TextMeshProUGUI>().text = obj.objectiveDescription;
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(questPanel.GetComponent<RectTransform>());
    }

    public void DisableQuestUI()
    {
        questPanel.SetActive(false);
        _questObjects.Clear();
        foreach (Transform obj in objectivePanel)
        {
            Destroy(obj.gameObject);
        }
    }
    
    public void UpdateQuestObjective(QuestObjective objective)
    {
        foreach (var obj in _questObjects)
        {
            if (obj.GetComponent<TextMeshProUGUI>().text == objective.objectiveDescription)
            {
                obj.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
            }
        }
    }
}