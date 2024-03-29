using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _musicSlider;
    [SerializeField]
    private Slider _sfxSlider;
    [SerializeField]
    private Toggle _postProcessingToggle;
    [SerializeField]
    private Toggle _trailsToggle;
    [SerializeField]
    private GameObject _postProcessing;
    [SerializeField]
    private GameObject[] _ballsPrefabs;
    private void Start()
    {
        //Since we want to save the player's preferences, we have to store them inside PlayerPrefs
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
        if (PlayerPrefs.HasKey("PostProcessing"))
        {
            _postProcessingToggle.isOn = IntToBool(PlayerPrefs.GetInt("PostProcessing"));
        }
        if (PlayerPrefs.HasKey("isTrails"))
        {
            _trailsToggle.isOn = IntToBool(PlayerPrefs.GetInt("isTrails"));
        }
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

    public void SetEffects(bool isPostprocessing)
    {
        _postProcessing.SetActive(isPostprocessing);
        PlayerPrefs.SetInt("PostProcessing", BoolToInt(isPostprocessing));
    }

    public void SetTrails(bool isTrails)
    {
        foreach(GameObject ball in _ballsPrefabs)
        {
            ball.GetComponent<TrailRenderer>().enabled = isTrails;
            PlayerPrefs.SetInt("isTrails", BoolToInt(isTrails));
        }
    }

    private bool IntToBool(int number)
    {
        if (number != 0) return true;
        return false;
    }
    private int BoolToInt(bool state)
    {
        if (state) return 1;
        return 0;
    }
}
