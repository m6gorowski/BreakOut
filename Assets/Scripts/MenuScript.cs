using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        SetMusicVolume(_musicSlider.value);
        SetSFXVolume(_sfxSlider.value);
    }
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("game closed");
    }
    public void StartButton()
    {
        FindObjectOfType<GameManagerScript>().NewGame();
    }

    public void SetMusicVolume(float volume)
    {
        //Because minimum audio is -80db, minimum volume is 0.0001 (log10 of 0.0001 is -4 and -4 * 20 is -80)
        //Audiomixer volume isn't linear, that's why I don't just put in volume with min -80 and max 0
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        _audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
}
