using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    [SerializeField]
    private Color[] _states;

    //is a number between 0 and 1, if it is 0.42, it means that there is 42% probability of dropping a powerUp when hit
    [SerializeField, Range(0f, 1f)]
    private float _powerUpProbability;
    public int health { get; private set; }
    public int points = 10;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public bool unbreakable;
    private void Awake()
    {
        this.SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        //if the brick isn't unbreakable, it sets it state on the start. The state is the color of the brick
        if (!this.unbreakable)
        {
            this.health = this._states.Length;
            this.SpriteRenderer.color = this._states[health - 1];
        }
    }

    /*Hit function changes the number of lives if the brick is hit and its breakable. It also changes it's state or sets unactive, 
     * based on the number of remaining lives. It also changes the score in the gamemanager. */
    private void Hit() 
    {
        if (unbreakable)
        {
            return;
        }

        this.health--;

        if(this.health <= 0)
        {
            PowerUpDrop();
            this.gameObject.SetActive(false);
        }
        else
        {
            this.SpriteRenderer.color = this._states[health - 1];
        }
        FindObjectOfType<GameManagerScript>().Hit(this);
    }

    private void PowerUpDrop()
    {
        if ( Random.Range(0f, 1f) <= this._powerUpProbability) 
        {
            FindObjectOfType<GameManagerScript>().SpawnPowerUp(this.transform.position);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            Hit();
        }
    }
}
