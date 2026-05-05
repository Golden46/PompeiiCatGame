using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    // Quest Sounds
    ECHO_SENSE,
    RESTORE,
    
    // Cat Sounds
    INTERACT,
    IDLE
}

[System.Serializable]
public class SoundEntry
{
    public SoundType type;
    public AudioClip[] clips;
    [Range(0f, 1f)] public float volume = 1f;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundEntry[] soundLibrary;

    private static AudioManager _instance;
    private AudioSource _audioSource;
    private Dictionary<SoundType, SoundEntry> _soundMap;

    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = GetComponent<AudioSource>();

        _soundMap = new Dictionary<SoundType, SoundEntry>();
        foreach (var entry in soundLibrary)
            _soundMap[entry.type] = entry;
    }

    public static void PlaySound(SoundType sound)
    {
        if (!_instance._soundMap.TryGetValue(sound, out var entry)) return;
        if (entry.clips.Length == 0) return;

        var clip = entry.clips[Random.Range(0, entry.clips.Length)];
        _instance._audioSource.PlayOneShot(clip, entry.volume);
    }
}
