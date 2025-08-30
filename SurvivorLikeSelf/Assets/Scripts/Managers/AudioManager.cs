using System;
using System.Collections.Generic;
using System.Threading;
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

    private UniTaskVoid _uniSoundVoid = new UniTaskVoid();
    private CancellationTokenSource _cancelMusicTrack = new CancellationTokenSource();

    int _currentSongIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        InitializeSources();
    }

    private void Start()
    {
        WaitForNextSong(_cancelMusicTrack).Forget();
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

    private void InitializeSources()
    {
        _musicSource.ignoreListenerPause = true;
        // Initialize General Audio Source Queues
        for (int i = 0; i < _generalSourcePoolSize; i++)
        {
            AudioSource newSource = _generalSourcesHolder.AddComponent<AudioSource>();
            _sourceStandbyQueue.Enqueue(newSource);
        }
    }

    private async UniTaskVoid WaitForNextSong(CancellationTokenSource cancellationToken, AudioClip newSong = null)
    {
        while (true)
        {
            PlayMusic(newSong);
            await UniTask.Delay(TimeSpan.FromSeconds(_musicClips[_currentSongIndex].length), ignoreTimeScale: true);
        }
    }

    private void PlayMusic(AudioClip newSong)
    {
        if (newSong == null)
        {
            _currentSongIndex++;

            if (_currentSongIndex >= _musicClips.Length) _currentSongIndex = 0;
            _musicSource.clip = _musicClips[_currentSongIndex];
            _musicSource.Play();
        }
        else
        {
            _musicSource.clip = newSong;
            _musicSource.Play();
        }
    }

    private void PlayNewSong(AudioClip newSong)
    {
        _cancelMusicTrack.Cancel();

        WaitForNextSong(_cancelMusicTrack, newSong).Forget();
    }
    

    // Try working with UniTask later so that the delay works with timescale
    public UniTaskVoid PlaySound(AudioClip sound)
    {
        AudioSource newSource = _sourceStandbyQueue.Dequeue();
        newSource.clip = sound;
        newSource.Play();

        UniTask.Delay((int)(sound.length * 1000f), ignoreTimeScale: false);

        _sourceStandbyQueue.Enqueue(newSource);

        return _uniSoundVoid;
    }
}
