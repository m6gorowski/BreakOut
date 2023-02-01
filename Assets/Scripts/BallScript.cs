using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    [SerializeField]
    private float _speed = 12f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        ResetBall();
    }
    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * _speed;
    }
    //Sets random trajectory the ball goes into - force.x is left/right, force.y is down, getting a random vector, which is normalised and applied to the ball at the start
    private void SetRandomTrajectory()
    {
        Vector2 force = Vector2.zero;
        force.x = Random.Range(-0.75f, 0.75f);
        force.y = -1f;
        this.rb.AddForce(force.normalized * this._speed);
    }
    //Resets the ball position and velocity and applies the trajectory
    public void ResetBall()
    {
        this.transform.position = Vector2.zero;
        this.rb.velocity = Vector2.zero;

        Invoke(nameof(SetRandomTrajectory), 1f);
    }
}
