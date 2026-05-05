using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    
    private readonly int _clipHeightPropertyID = Shader.PropertyToID("_ClipHeight");
    public Material holoMaterial;

    [Header("General Quest Variables")]
    public QuestUI questUI;
    public Quest activeQuest;
    public QuestState activeQuestState = QuestState.Inactive;
    public List<GameObject> questBuilding;
    
    [Header("For Quest Finish")]
    private CinemachineVirtualCamera _questCamera;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float dollyCinematicDuration = 3f;
    [SerializeField] private float fadeDuration = 2f;
    
    [Header("Subtitles")]
    [FormerlySerializedAs("cantCollectItemMessages")] public string[] itemCollectErrorMessages;
    public string[] itemCollectMessages;
    
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
                return false;
            case QuestState.Completed:
                DialogueManager.Instance.StartDialogue(activeQuest.catDialogue, FinishQuest, DialogueType.End);
                return true;
            default:
                return true;
        }
    }

    public void StartQuest(Quest quest)
    {
        activeQuest = quest;
        activeQuestState = QuestState.Active;
        
        questUI.EnableQuestUI();
    }

    public void HoloRestoration(CinemachineVirtualCamera questCamera,  List<Renderer> holoStructure)
    {
        _questCamera = questCamera;
        const float duration = 9.5f;
        const float startHeight = 15f;
        const float endHeight = 25f;
        foreach (var r in holoStructure)
        {
            StartCoroutine(Verticality(r, duration, startHeight, endHeight, questCamera));
        }
    }

    private IEnumerator Verticality(Renderer targetRenderer, float duration, float startHeight, float endHeight,
        CinemachineVirtualCamera questCamera)
    {
        // Clone all material slots with the hologram material
        var materials = targetRenderer.materials;
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].shader == holoMaterial.shader)
                materials[i] = new Material(materials[i]);
        }
        targetRenderer.materials = materials;

        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var currentHeight = Mathf.Lerp(startHeight, endHeight, elapsedTime / duration);

            foreach (var m in targetRenderer.materials)
            {
                if (m.shader == holoMaterial.shader)
                    m.SetFloat(_clipHeightPropertyID, currentHeight);
            }
            yield return null;
        }

        foreach (var m in targetRenderer.materials)
        {
            if (m.shader == holoMaterial.shader)
                m.SetFloat(_clipHeightPropertyID, endHeight);
        }
    }
    
    public Quest GetActiveQuest()
    {
        return activeQuest;
    }
    
    public bool CheckObjective(string itemID)
    {
        if (activeQuestState == QuestState.Inactive) return false; // If there is no quest then the object cannot be collected.
        foreach (var obj in activeQuest.objectives) // For all the current objectives needed
        {
            if (obj.itemID == itemID)  // If the item trying to be collected is an objective
            {
                // If there is a required item to complete the obj or they don't have the required item then return.
                if (obj.requiredItemID != "" && !_collectedItems.Contains(obj.requiredItemID))
                    return false;
                
                // Else collect the item.
                _collectedItems.Add(itemID);
                return CompleteObjective(obj);

            }
            
            if (obj.requiredItemID == itemID) // If the item isn't an obj but a required objective then collect it
            { 
                _collectedItems.Add(itemID);
                return true;
            }
        }
        
        // If none of this then don't collect
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
        
        AudioManager.PlaySound(SoundType.RESTORE);
        StartCoroutine(BuildingTransition());
    }
    
    private IEnumerator BuildingTransition()
    {
        _questCamera.gameObject.SetActive(true);

        // Makes sure the dolly camera plays a little before the fade
        yield return new WaitForSeconds(dollyCinematicDuration);

        // Fade screen to black
        yield return StartCoroutine(Fade(0f, 1f));

        // Swaps out the ruins for the complete building
        questBuilding[0].SetActive(false);
        questBuilding[1].SetActive(true);
        
        // Pause before fade back in
        yield return new WaitForSeconds(fadeDuration/2);
        
        // Fade screen back in
        yield return StartCoroutine(Fade(1f, 0f));
        
        // Another pause so the dolly camera doesn't exit out too early.
        yield return new WaitForSeconds(dollyCinematicDuration);
        
        _questCamera.gameObject.SetActive(false);
    }

    private IEnumerator Fade(float from, float to)
    {
        var elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(from, to, t));
            yield return null;
        }
        fadeImage.color = new Color(0f, 0f, 0f, to);
    }
}