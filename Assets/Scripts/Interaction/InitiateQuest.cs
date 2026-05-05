using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

public class InitiateQuest : MonoBehaviour
{
    private QuestManager _questManager;

    public GameObject structureParent;
    private Renderer[] _objects;
    public List<Renderer> holoStructure;

    public CatAIStateMachine cat;
    public Quest catQuest;
    public CinemachineVirtualCamera questCamera;
    [SerializeField] private GameObject[] questBuilding; // 0 Slot for the ruin and 1 slot for the full build.

    private void Start()
    {
        _questManager = QuestManager.Instance;  
    }
    
    // Acts as the gate between getting the quest
    public void QuestGate()
    {
        var questDone = _questManager.CheckQuestState(catQuest); // Checks if the Quest can be given to the player
        if (questDone) return;
        
        // If it can then it gets the objects in the building with the hologram components
        _objects = structureParent.GetComponentsInChildren<Renderer>();
        foreach (var r in _objects)
        {
            if (r.sharedMaterials.Any(m => m == _questManager.holoMaterial))
                holoStructure.Add(r);
        }
        
        AudioManager.PlaySound(SoundType.INTERACT);
        DialogueManager.Instance.StartDialogue(catQuest.catDialogue, OnDialogueComplete, DialogueType.Start);
    }

    private void OnDialogueComplete()
    {
        _questManager.StartQuest(catQuest);
        
        // Builds up the hologram
        _questManager.HoloRestoration(questCamera, holoStructure);
        foreach(var b in  questBuilding) _questManager.questBuilding.Add(b);
        
        //Instantiate(_echoSensePrefab, transform.position, _echoSensePrefab.transform.rotation);
        AudioManager.PlaySound(SoundType.ECHO_SENSE);
    }
}