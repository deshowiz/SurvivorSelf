using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixControl : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _masterAudio = null;
    [SerializeField]
    private Slider _masterVSlider = null;
    [SerializeField]
    private Slider _soundFXSlider = null;
    [SerializeField]
    private Slider _musicSlider = null;

    public void UpdateSettingsVisuals()
    {
        float newValue;
        _masterAudio.GetFloat("MasterVolume", out newValue);
        _masterVSlider.value = Mathf.Pow(10f, newValue / 20f);

        _masterAudio.GetFloat("SoundFXVolume", out newValue);
        _soundFXSlider.value = Mathf.Pow(10f, newValue / 20f);

        _masterAudio.GetFloat("MusicVolume", out newValue);
        _musicSlider.value = Mathf.Pow(10f, newValue / 20f);
    }

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
