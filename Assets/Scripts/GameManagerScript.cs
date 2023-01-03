using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int level = 1;
    public int lives = 3;
    public int score = 0;
    [SerializeField]
    private int _maxLevelAmount = 5;

    public BallScript ball { get; private set; }
    public PaddleScript paddle { get; private set; }
    public BrickScript[] bricks { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void Start()
    {
        NewGame();
    }
    private void NewGame()
    {
        this.score = 0;
        this.lives = 3;
        LoadLevel(1);
    }
    public void LoadLevel(int level)
    {
        this.level = level;
        if(level > _maxLevelAmount)
        {
            SceneManager.LoadScene("FinishScene");
        }
        SceneManager.LoadScene("Level" + level);
    }
    private void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();
    }
    private void GameOver()
    {
        //SceneManager.LoadLevel("GameOver");
        NewGame();
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        this.ball = FindObjectOfType<BallScript>();
        this.paddle = FindObjectOfType<PaddleScript>();
        this.bricks = FindObjectsOfType<BrickScript>();
    }
    public void Miss()
    {
        this.lives--;
        if (this.lives > 0)
        {
            ResetLevel();
        }
        else
        {
            GameOver();
        }
    }
    public void Hit(BrickScript brick)
    {
        this.score += brick.points;
        if (ClearedLevel())
        {
            LoadLevel(this.level + 1);
        }
    }
    private bool ClearedLevel()
    {
        for(int i = 0; i < this.bricks.Length; i++)
        {
            if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
            {
                return false;
            }
        }
        return true;
    }
}
