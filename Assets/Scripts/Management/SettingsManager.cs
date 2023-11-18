using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public AudioMixer mainMixer;
    public Slider musicSlider, sfxSlider;

    private void Start()
    {
        mainMixer.GetFloat("MusicVol", out float vol);
        musicSlider.value = vol;
        mainMixer.GetFloat("SfxVol", out float vol2);
        sfxSlider.value = vol2;
    }

    public void OnMusicSlide()
    {
        mainMixer.SetFloat("MusicVol", musicSlider.value);
    }

    public void OnSfxSlide()
    {
        mainMixer.SetFloat("SfxVol", sfxSlider.value);
    }
}
