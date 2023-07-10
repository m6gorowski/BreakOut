using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    private bool isPaused;
    void Awake()
    {
        DontDestroyOnLoad(this);
        this.gameObject.SetActive(false);
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
    }
    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void BackToMenu()
    {
        ResumeGame();
        //To avoid having multiple Pause Menus showing up or GameManagers playing music, I have to destroy them/set them unactive. New ones are default on the Menu.
        Destroy(this.gameObject);
        FindObjectOfType<GameManagerScript>().gameObject.SetActive(false);
        SceneManager.LoadScene("StartMenu");
    }
}
