using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    [SerializeField]
    private float _speed = 30f;
    [SerializeField]
    private float maxBounceAngle = 75f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void ResetPaddle()
    {
        this.transform.position = new Vector2(0f, this.transform.position.y);
        this.rb.velocity = Vector2.zero;

    }

    private void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), 0);
    }
    private void FixedUpdate()
    {
        if(this.direction != Vector2.zero)
        {
            this.rb.AddForce(this.direction * this._speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        BallScript ball = other.gameObject.GetComponent<BallScript>();
        if (ball != null)
        {
            Vector3 paddlePosition = this.transform.position;
            Vector2 contactPoint = other.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float width = other.otherCollider.bounds.size.x / 2;

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rb.velocity);
            float bounceAngle = (offset / width) * this.maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
            ball.rb.velocity = rotation * Vector2.up * ball.rb.velocity.magnitude;
        }
    }
}
