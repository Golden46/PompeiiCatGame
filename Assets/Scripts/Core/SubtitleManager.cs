using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private Transform subtitlePanel;
    [SerializeField] private Transform subtitlePrefab;
    
    private List<Transform> _activeSubtitles = new();
    
    private readonly float[] _subtitleAlphas = { 100f/255f, 160f/255f, 1f };
    
    private const float SubtitleUptime = 5.0f;
    public bool dialogueInProgress;
    private float _countdown;
    
    public static SubtitleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Update()
    {
        // Countdown for the time subtitle should be on screen. Doesn't run for NPC dialogue
        if (_activeSubtitles.Count <= 0 || dialogueInProgress) return;
        _countdown -= Time.deltaTime;

        if (!(_countdown <= 0)) return;
        
        DeleteAllSubtitles();
    }
    
    public void ShowSubtitle(string text)
    {
        if (dialogueInProgress) return;
        
        _countdown = SubtitleUptime; // Reset countdown time every time a new subtitle is added.
        
        CheckSubtitlesAmount(3); // No more than 3 subtitles should be active at the same time.
        
        // Instantiate subtitle and add it to the list
        var subtitle = ShowText(text, Color.white);
        _activeSubtitles.Add(subtitle);

        // Fade away subtitles as new ones are added.
        RefreshSubtitleAlphas();
    }
    
    // To show current dialogue
    public void ShowDialogue(string text, Color dialogueColor)
    {
        dialogueInProgress = true;

        DeleteAllSubtitles();

        var dialogue = ShowText(text, dialogueColor);
        _activeSubtitles.Add(dialogue);
    }
    
    
    // Spawns subtitle prefab into scene and populates text
    private Transform ShowText(string text, Color color)
    {
        var subtitle = Instantiate(subtitlePrefab, subtitlePanel.transform); // Spawn text prefab
        subtitle.GetComponentInChildren<TextMeshProUGUI>().text = text; // Set the text
        subtitle.GetComponentInChildren<TextMeshProUGUI>().color = color;
            
        return subtitle;
    }
    
    // Checks how many subtitles are currently on screen
    private void CheckSubtitlesAmount(int amount)
    {
        if (_activeSubtitles.Count < amount) return;
        
        Destroy(_activeSubtitles[0].gameObject);
        _activeSubtitles.RemoveAt(0);
        RefreshSubtitleAlphas();
    }
    
    // Deletes all active subtitles.
    private void DeleteAllSubtitles()
    {
        foreach(var s in  _activeSubtitles.ToList())
        {
            Destroy(s.gameObject);
            _activeSubtitles.Remove(s);
        }
    }
    
    // Makes it so the subtitles look like they are fading out by manipulating the alpha of the text colour
    private void RefreshSubtitleAlphas()
    {
        for (var i = 0; i < _activeSubtitles.Count; i++)
        {
            var tmp = _activeSubtitles[i].GetComponentInChildren<TextMeshProUGUI>();
            var c = tmp.color;
            c.a = _subtitleAlphas[i + (3 - _activeSubtitles.Count)]; 
            tmp.color = c;
        }
    }
}
