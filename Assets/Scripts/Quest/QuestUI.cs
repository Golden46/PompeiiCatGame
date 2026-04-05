using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private Transform objectivePanel;
    [SerializeField] private Texture tickedBox;

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescText;
    private readonly List<GameObject> _questObjects = new List<GameObject>();

    private QuestManager _questManager;

    void Start()
    {
        _questManager = FindAnyObjectByType<QuestManager>();
    }

    public void EnableQuestUI()
    {
        questPanel.SetActive(true);
        Quest quest = _questManager.GetActiveQuest();
        questTitleText.text = quest.title;
        questDescText.text = quest.description;

        foreach (QuestObjective obj in quest.objectives) {
            GameObject objective = Instantiate(objectivePrefab, objectivePanel.position, Quaternion.identity, objectivePanel);
            _questObjects.Add(objective);
            objective.GetComponentInChildren<TextMeshProUGUI>().text = obj.objectiveDescription;
        }
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
        foreach (GameObject obj in _questObjects)
        {
            if (obj.GetComponentInChildren<TextMeshProUGUI>().text == objective.objectiveDescription)
            {
                obj.GetComponentInChildren<RawImage>().texture = tickedBox;
            }
        }
    }
}