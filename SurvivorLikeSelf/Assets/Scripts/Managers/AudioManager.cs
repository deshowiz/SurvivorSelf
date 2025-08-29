using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    [SerializeField]
    private AudioSource _musicSource = null;
    [SerializeField]
    private AudioClip[] _musicClips = new AudioClip[0];

    [Header("General Audio")]
    [SerializeField]
    private Transform _generalSourcesHolder = null;
    [SerializeField]
    private int _generalSourcePoolSize = 50;
    private Queue<AudioSource> _sourceStandbyQueue = new Queue<AudioSource>();

    private float _newMusicEndTime = 0f;

    private float _currentAudioSystemTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InitializeSources();
    }

    private void OnEnable()
    {
        EventManager.OnPausedGame += PauseListener;
        EventManager.OnResumedGame += ResumeListener;
    }
    
    private void OnDisable()
    {
        EventManager.OnPausedGame -= PauseListener;
        EventManager.OnResumedGame -= ResumeListener;
    }

    private void PauseListener()
    {
        AudioListener.pause = true;
    }

    private void ResumeListener()
    {
        AudioListener.pause = false;
    }

    private void Update()
    {
        _currentAudioSystemTime = (float)AudioSettings.dspTime;
        if (_currentAudioSystemTime > _newMusicEndTime)
        {
            PlayMusic();
        }
    }

    private void InitializeSources()
    {
        _musicSource.ignoreListenerPause = true;
        // Initialize General Audio Source Queues
        for (int i = 0; i < _generalSourcePoolSize; i++)
        {
            AudioSource newSource = _generalSourcesHolder.AddComponent<AudioSource>();
            //newSource.ignoreListenerPause = false;
            _sourceStandbyQueue.Enqueue(newSource);
        }
    }

    private void PlayMusic(AudioClip song = null)
    {
        if (song)
        {
            _musicSource.clip = song;
            _newMusicEndTime = _currentAudioSystemTime + song.length;
        }
        else
        {
            _musicSource.clip = _musicClips[0];
            _newMusicEndTime = _currentAudioSystemTime + _musicClips[0].length;
        }

        _musicSource.Play();
    }

    // Try working with UniTask later so that the delay works with timescale
    public UniTaskVoid PlaySound(AudioClip sound)
    {
        AudioSource newSource = _sourceStandbyQueue.Dequeue();
        newSource.clip = sound;
        newSource.Play();

        UniTask.Delay((int)(sound.length * 1000f), ignoreTimeScale: false);

        _sourceStandbyQueue.Enqueue(newSource);

        return new UniTaskVoid();
    }
}
