using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class StartMenuScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private Slider _audioSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            _audioSlider.value = PlayerPrefs.GetFloat("masterVolume");
        }
        SetVolume(_audioSlider.value);
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

    public void SetVolume(float volume)
    {
        //Because minimum audio is -80db, minimum volume is 0.0001 (log10 of 0.0001 is -4 and -4 * 20 is -80)
        //Audiomixer volume isn't linear, that's why I don't just put in volume with min -80 and max 0
        _audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

}
