using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    private bool isPaused;
    public AudioManagerScript AudioManagerScript { get; private set; }
    void Awake()
    {
        DontDestroyOnLoad(this);
        this.gameObject.SetActive(false);
        this.AudioManagerScript = GameObject.FindObjectOfType<AudioManagerScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                return;
            }
            PauseGame();
        }
    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioManagerScript.musicSource.Pause();
    }
    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        AudioManagerScript.musicSource.UnPause();
    }
    public void BackToMenu()
    {
        ResumeGame();
        //To avoid having multiple Pause Menus showing up or GameManagers playing music, I have to destroy them/set them unactive. New ones are default on the Menu.
        Destroy(this.gameObject);
        FindObjectOfType<GameManagerScript>().gameObject.SetActive(false);
        SceneManager.LoadScene("StartMenu");
    }
    public void SetTrailsToActiveBalls(bool isTrails)
    {
        BouncyBallScript[] bouncyBalls = GameObject.FindObjectsOfType<BouncyBallScript>();
        BallScript[] balls = GameObject.FindObjectsOfType<BallScript>();
        ExtraBallScript[] extraBalls = GameObject.FindObjectsOfType<ExtraBallScript>();
        foreach (BouncyBallScript bB in bouncyBalls)
        {
            bB.GetComponent<TrailRenderer>().enabled = isTrails;
        }
        foreach (BallScript b in balls)
        {
            b.GetComponent<TrailRenderer>().enabled = isTrails;
        }
        foreach (ExtraBallScript eB in extraBalls)
        {
            eB.GetComponent<TrailRenderer>().enabled = isTrails;
        }

    }
}
