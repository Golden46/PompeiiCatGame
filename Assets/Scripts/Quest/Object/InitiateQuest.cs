using UnityEngine;
using Cinemachine;

public class InitiateQuest : MonoBehaviour
{
    private QuestManager _questManager;
    
    public GameObject[] holoStructure;
    
    public GameObject cat;
    public Quest catQuest;

    public CinemachineVirtualCamera questCamera;

    private void Start()
    {
        _questManager = QuestManager.Instance;  
    }

    public void QuestGate()
    {
        var quest = _questManager.CheckQuestState(catQuest);
        if (quest) return;
        _questManager.HoloRestoration(questCamera, holoStructure);
        //Instantiate(_echoSensePrefab, transform.position, _echoSensePrefab.transform.rotation);
        AudioManager.PlaySound(SoundType.ECHOSENSE, 0.25f);
    }
}