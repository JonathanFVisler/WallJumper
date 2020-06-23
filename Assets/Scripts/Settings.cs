using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider effectSlider;

    public void LoadSettings()
    {
        musicSlider.value = Memory.MusicVolume;
        effectSlider.value = Memory.EffectVolume;
    }

    public void SaveSettings()
    {
        Memory.MusicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        Memory.EffectVolume = effectSlider.value;
        PlayerPrefs.SetFloat("EffectVolume", effectSlider.value);
    }
}
