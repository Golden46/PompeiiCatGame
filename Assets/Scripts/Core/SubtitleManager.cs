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
    private float _countdown;
    
    // 100, 160, 255 alphas
    
    public static SubtitleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Update()
    {
        if (_activeSubtitles.Count > 0)
        {
            _countdown -= Time.deltaTime;
            
            if (_countdown <= 0)
            {
                foreach(var s in  _activeSubtitles.ToList())
                {
                    Destroy(s.gameObject);
                    _activeSubtitles.Remove(s);
                }
            } 
        }
    }
    
    public void ShowSubtitle(string text)
    {
        _countdown = SubtitleUptime; // Reset countdown time every time a new subtitle is added.
        
        if (_activeSubtitles.Count >= 3) // No more than 3 subtitles should be active at the same time.
        {
            Destroy(_activeSubtitles[0].gameObject);
            _activeSubtitles.RemoveAt(0);
            RefreshSubtitleAlphas(); 
        }
        
        // Instantiate subtitle and add it to the list
        var subtitle = Instantiate(subtitlePrefab, subtitlePanel.transform);
        _activeSubtitles.Add(subtitle);
        
        // Set subtitle text.
        subtitle.GetComponentInChildren<TextMeshProUGUI>().text = text;

        // Fade away subtitles as new ones are added.
        RefreshSubtitleAlphas();
    }
    
    private void RefreshSubtitleAlphas()
    {
        for (int i = 0; i < _activeSubtitles.Count; i++)
        {
            var tmp = _activeSubtitles[i].GetComponentInChildren<TextMeshProUGUI>();
            Color c = tmp.color;
            c.a = _subtitleAlphas[i + (3 - _activeSubtitles.Count)]; // offset so newest is always 1f
            tmp.color = c;
        }
    }
}
