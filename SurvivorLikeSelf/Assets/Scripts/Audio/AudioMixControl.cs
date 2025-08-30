using UnityEngine;
using UnityEngine.Audio;

public class AudioMixControl : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _masterAudio = null;

    public void SetMasterVolume(float newVolume)
    {
        _masterAudio.SetFloat("MasterVolume", Mathf.Log10(newVolume) * 20f);
    }

    public void SetSoundFXVolume(float newVolume)
    {
        _masterAudio.SetFloat("SoundFXVolume", Mathf.Log10(newVolume) * 20f);
    }

    public void SetMusicVolume(float newVolume)
    {
        _masterAudio.SetFloat("MusicVolume", Mathf.Log10(newVolume) * 20f);
    }
}
