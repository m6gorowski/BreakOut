using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{
    [SerializeField]
    private Color[] _states;
    public int health { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }
    [SerializeField]
    private bool unbreakable;
    private void Awake()
    {
        this.SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (!this.unbreakable)
        {
            this.health = this._states.Length;
            this.SpriteRenderer.color = this._states[health - 1];
        }
    }
    private void Hit() 
    {
        if (unbreakable)
        {
            return;
        }

        this.health--;

        if(this.health <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.SpriteRenderer.color = this._states[health - 1];
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Ball")
        {
            Hit();
        }
    }
}
