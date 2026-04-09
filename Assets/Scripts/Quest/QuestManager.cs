using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private readonly int _clipHeightPropertyID = Shader.PropertyToID("_ClipHeight");

    public QuestUI questUI;
    
    public Quest activeQuest;
    public QuestState activeQuestState = QuestState.Inactive;
    
    private HashSet<string> _collectedItems = new(); // Used to see if certain objectives can be completed.
    private float _completedObjs;
    
    private readonly HashSet<Quest> _finishedQuests = new(); 

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        questUI = GetComponent<QuestUI>();
    }

    public bool CheckQuestState(Quest quest)
    {
        if (_finishedQuests.Contains(quest)) return true;
        switch (activeQuestState)
        {
            case QuestState.Inactive:
                StartQuest(quest);
                return false;
            case QuestState.Completed:
                FinishQuest();
                break;
        }

        return true;
    }

    private void StartQuest(Quest quest)
    {
        activeQuest = quest;
        activeQuestState = QuestState.Active;
        
        questUI.EnableQuestUI();
    }

    public void HoloRestoration(CinemachineVirtualCamera questCamera, GameObject[] holoStructure)
    {
        questCamera.gameObject.SetActive(true); // Enables the dolly camera
        const float duration = 9.5f;
        const float startHeight = 0f;
        const float endHeight = 2.5f;
        foreach (var structure in holoStructure)
        {
            var targetRenderer = structure.GetComponent<Renderer>();
            StartCoroutine(Verticality(targetRenderer, duration, startHeight, endHeight, questCamera));
        }
    }

    private IEnumerator Verticality(Renderer targetRenderer, float duration, float startHeight, float endHeight,
        CinemachineVirtualCamera questCamera)
    {
        targetRenderer.material = new Material(targetRenderer.material);

        var elapsedTime = 0f;

        targetRenderer.material.SetFloat(_clipHeightPropertyID, startHeight);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            var currentHeight = Mathf.Lerp(startHeight, endHeight, elapsedTime / duration);

            targetRenderer.material.SetFloat(_clipHeightPropertyID, currentHeight);
            yield return null;
        }

        questCamera.gameObject.SetActive(false); // Disables the dolly camera
        targetRenderer.material.SetFloat(_clipHeightPropertyID, endHeight);
    }

    public Quest GetActiveQuest()
    {
        return activeQuest;
    }
    
    public bool CheckObjective(string itemID)
    {
        if (activeQuestState == QuestState.Inactive) return false;
        foreach (var obj in activeQuest.objectives)
        {
            if (obj.itemID == itemID)
            {
                if (obj.requiredItemID == "" || _collectedItems.Contains(obj.requiredItemID)) return CompleteObjective(obj);
                
                return false;
            }
            
            if (obj.requiredItemID == itemID)
            { 
                _collectedItems.Add(itemID);
                return true;
            }
        }
        
        return false;
    }

    private bool CompleteObjective(QuestObjective obj)
    {
        _completedObjs++;
        questUI.UpdateQuestObjective(obj);
        if (_completedObjs >= activeQuest.objectives.Length) CompleteQuest();
        return true;
    }
    
    private void CompleteQuest()
    {
        activeQuestState = QuestState.Completed;
        Debug.Log("Quest completed: " + activeQuest.title);
    }
    
    private void FinishQuest()
    {
        activeQuestState = QuestState.Inactive;
        _finishedQuests.Add(activeQuest);
        _completedObjs = 0;
        questUI.DisableQuestUI();
    }
}