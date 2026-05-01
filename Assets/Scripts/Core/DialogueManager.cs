using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    private DialogueLine[] _lines;
    private int _currentIndex;
    private Action _onComplete;
    private bool _isActive;

    private Color _dialogueColor;
    
    [SerializeField] private InputActionReference advanceDialogue;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
 
    private void OnEnable()
    {
        if (advanceDialogue != null)
            advanceDialogue.action.performed += OnAdvance;
    }
 
    private void OnDisable()
    {
        if (advanceDialogue != null)
            advanceDialogue.action.performed -= OnAdvance;
    }

    public void StartDialogue(CatDialogue catDialogue, Action onComplete)
    {
        if (catDialogue == null || catDialogue.lines.Length == 0)
        {
            onComplete?.Invoke();
            return;
        }
 
        _lines = catDialogue.lines;
        _dialogueColor = catDialogue.catColor;
        _currentIndex = 0;
        _onComplete = onComplete;
        _isActive = true;
 
        ShowCurrentLine();
    }
    
    private void OnAdvance(InputAction.CallbackContext context)
    {
        if (!_isActive) return;
 
        _currentIndex++;
 
        if (_currentIndex >= _lines.Length)
        {
            _isActive = false;
            SubtitleManager.Instance.dialogueInProgress = false;
            _onComplete?.Invoke();
            return;
        }
 
        ShowCurrentLine();
    }
    
    private void ShowCurrentLine()
    {
        var line = _lines[_currentIndex];
        var formatted = $"{line.speaker}: {line.text}";
        SubtitleManager.Instance.ShowDialogue(formatted, _dialogueColor);
    }
}
