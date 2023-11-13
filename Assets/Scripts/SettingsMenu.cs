using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixer SFX;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
    public void SetSFXVolume(float sfxvolume)
    {
        SFX.SetFloat("SFX", sfxvolume);
    }
}
