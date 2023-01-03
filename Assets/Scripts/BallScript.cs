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
    private void SetRandomTrajectory()
    {
        Vector2 force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = -1f;
        this.rb.AddForce(force.normalized * this._speed);
    }
    public void ResetBall()
    {
        this.transform.position = Vector2.zero;
        this.rb.velocity = Vector2.zero;

        Invoke(nameof(SetRandomTrajectory), 1f);
    }
}