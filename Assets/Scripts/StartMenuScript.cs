using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("game closed");
    }
    public void StartButton()
    {
        FindObjectOfType<GameManagerScript>().NewGame();
    }

}
