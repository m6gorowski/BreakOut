using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //This part is for testing only!
    [SerializeField]
    private int _startingLevel = 1;

    public int level = 1;
    public int lives = 3;
    public int score = 0;
    
    [SerializeField]
    private int _maxLevelAmount = 5;

    //0 - TripleBallPowerUp
    [SerializeField]
    private GameObject[] PowerUps;

    public float extraBallOffset = 2f;
    [SerializeField]
    private GameObject _BallPref;
    [SerializeField]
    private GameObject _extraBallPref;
    [SerializeField]
    private GameObject _bouncyBallPref;

    public BallScript ball { get; private set; }
    public PaddleScript paddle { get; private set; }
    public BrickScript[] bricks { get; private set; }
    public PowerUpScript[] powerUps { get; private set; }

    public ShowLivesScript livesScript { get; private set; }

    [SerializeField]
    private float _powerUpTime;
    [SerializeField]
    private float _bigPaddleScale;
    [SerializeField]
    private float _speedMultiplier;
    [SerializeField]
    private GameObject _pauseCanvas;

    private void Awake()
    {
        //DontDestroyOnLoad makes the gameObject apparent on every level.
        DontDestroyOnLoad(this.gameObject);
        //gets all the bricks, the ball and the paddle from the level every time a scene is loaded + stops coroutines connected to powerups
        SceneManager.sceneLoaded += OnLevelLoaded;
    }
    private void Start()
    {
        //NewGame();
    }
    public void NewGame()
    {
        //Restarts the game
        _pauseCanvas.SetActive(true);
        this.score = 0;
        this.lives = 3;
        LoadLevel(_startingLevel);
    }
    public void LoadLevel(int level)
    {
        //loads levels, all of the playable levels go by name "Level" + the number
        this.level = level;
        if (level > _maxLevelAmount)
        {
            _pauseCanvas.SetActive(false);
            SceneManager.LoadScene("FinishScene");
            return;
        }
        SceneManager.LoadScene("Level" + level);
    }
    private void ResetLevel()
    {
        //resets the position, scale and velocity of the paddle and ball
        this.powerUps = FindObjectsOfType<PowerUpScript>();
        BouncyBallScript[] _bouncyBalls = FindObjectsOfType<BouncyBallScript>();
        foreach(PowerUpScript powerup in this.powerUps)
        {
            Destroy(powerup.gameObject);
        }
        foreach(BouncyBallScript ball in _bouncyBalls)
        {
            Destroy(ball.gameObject);
        }
        Instantiate(_BallPref, Vector2.zero, Quaternion.identity);
        this.paddle.ResetPaddle();
    }
    private void GameOver()
    {
        //SceneManager.LoadLevel("GameOver");
        NewGame();
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //find the bricks, the ball and the paddle gameobjects as the scene loads
        this.ball = FindObjectOfType<BallScript>();
        this.paddle = FindObjectOfType<PaddleScript>();
        this.bricks = FindObjectsOfType<BrickScript>();
        this.livesScript = FindObjectOfType<ShowLivesScript>();
        if(this.livesScript != null)
        {
            SetLives(this.lives) ;
        }
        StopAllCoroutines();
    }
    public void Miss()
    {
        //is called when the ball gets in the DeadZone - makes the number of player lives go down and it either restarts the level or the game
        StopAllCoroutines(); //it's for the longPaddle power up routine, so the paddle doesn't get smaller after the restart
        this.lives--;
        SetLives(this.lives);
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
        //adds points if the brick was hit
        //also checks if there are any blocks left
        this.score += brick.points;
        if (ClearedLevel())
        {
            LoadLevel(this.level + 1);
        }
    }
    private bool ClearedLevel()
    {
        //when the brick is hit, it gets called and checks every brick the gameManager got at the load of the scene if they are active
        //if one of them is active, it returns false and stops the function
        for(int i = 0; i < this.bricks.Length; i++)
        {
            if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
            {
                return false;
            }
        }
        return true;
    }

    //When a brick is hit and the random number is within the possibility (it's in the brick script), it gets called.
    //it selects a random powerUp from the table of powerUps and spawns it where the brick is
    public void SpawnPowerUp(Vector3 brickPos)
    {
        int powerUpIndex = Random.Range(0, PowerUps.Length);
        Instantiate(PowerUps[powerUpIndex], brickPos, Quaternion.identity);
    }
    public void PowerUpActive(int index)
    {
        if (index == 0)
        {
            TripleBallPowerUp();
        }
        else if(index == 1)
        {
            StartCoroutine(LongPaddlePowerUp());
        }
        else if(index == 2)
        {
            BouncyBallPowerUp();
        }
        else if (index == 3)
        {
            StartCoroutine(SpeedPowerUp());
        }
    }
    private IEnumerator LongPaddlePowerUp()
    {
        paddle.transform.localScale = new Vector3(_bigPaddleScale * paddle.transform.localScale.x, paddle.transform.localScale.y, paddle.transform.localScale.z);
        yield return new WaitForSeconds(_powerUpTime);
        paddle.transform.localScale = new Vector3(paddle.transform.localScale.x / _bigPaddleScale, paddle.transform.localScale.y, paddle.transform.localScale.z);
    }
    private void TripleBallPowerUp() 
    {
        Instantiate(_extraBallPref, new Vector2(paddle.transform.position.x + 0.5f, paddle.transform.position.y + extraBallOffset), Quaternion.identity);
        Instantiate(_extraBallPref, new Vector2(paddle.transform.position.x - 0.5f, paddle.transform.position.y + extraBallOffset), Quaternion.identity);
    }
    private void BouncyBallPowerUp() 
    {
        Instantiate(_bouncyBallPref, new Vector2(paddle.transform.position.x, paddle.transform.position.y + extraBallOffset), Quaternion.identity);
    }
    private IEnumerator SpeedPowerUp()
    {
        paddle._speed *= _speedMultiplier;
        yield return new WaitForSeconds(_powerUpTime);
        paddle._speed /= _speedMultiplier;
    }

    private void SetLives(int lives)
    {
        Renderer[] livesColors = livesScript.GetComponentsInChildren<Renderer>();
        for(int i = 0; i < 3; i++)
        {
            livesColors[i].material.color = livesScript._aliveColor;
        }
        for(int i = lives; i < 3; i++)
        {
            livesColors[i].material.color = livesScript._deadColor;
        }
    }
}
