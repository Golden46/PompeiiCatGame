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

    public Quest catQuest;
    public CinemachineVirtualCamera questCamera;
    [SerializeField] private GameObject[] questBuilding; // 0 Slot for the ruin and 1 slot for the full build.

    private void Start()
    {
        _questManager = QuestManager.Instance;  
    }

    public void QuestGate()
    {
        var quest = _questManager.CheckQuestState(catQuest);
        if (quest) return;

        _objects = structureParent.GetComponentsInChildren<Renderer>();
        foreach (var r in _objects)
        {
            if (r.sharedMaterials.Any(m => m == _questManager.holoMaterial))
                holoStructure.Add(r);
        }
        
        _questManager.HoloRestoration(questCamera, holoStructure);
        foreach(var b in  questBuilding) _questManager.questBuilding.Add(b);
        
        //Instantiate(_echoSensePrefab, transform.position, _echoSensePrefab.transform.rotation);
        AudioManager.PlaySound(SoundType.ECHOSENSE, 0.25f);
    }
}