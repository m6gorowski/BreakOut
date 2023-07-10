using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector3 normalScale { get; private set; }
    public float _speed = 35f;
    [SerializeField]
    private float _normalSpeed = 35f;
    [SerializeField]
    private float maxBounceAngle = 75f;
    public AudioManagerScript AudioManagerScript { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        normalScale = this.transform.localScale;
        this.AudioManagerScript = GameObject.FindObjectOfType<AudioManagerScript>();
    }
    public void ResetPaddle()
    {
        this.transform.position = new Vector2(0f, this.transform.position.y);
        this.rb.velocity = Vector2.zero;
        this.transform.localScale = normalScale;
        this._speed = _normalSpeed;
    }

    private void Update()
    {
        //gets the players input about left/right buttons
        direction = new Vector2(Input.GetAxis("Horizontal"), 0);
    }
    private void FixedUpdate()
    {
        //if the player input is different than zero, it adds force to move the paddle
        if(this.direction != Vector2.zero)
        {
            this.rb.AddForce(this.direction * this._speed);
        }
    }

    //collision mechanics with the ball
    private void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.name != "Wall" && other.gameObject.tag != "PowerUp")
        {
            AudioManagerScript.PlaySFX(AudioManagerScript.paddleHit);
        }
        //get the script of the ball, since we change it's values
        BallScript ball = other.gameObject.GetComponent<BallScript>();
        ExtraBallScript extraBall = other.gameObject.GetComponent<ExtraBallScript>();
        //if the ball is the object the paddle hit, do the following
        if (ball != null || extraBall != null)
        {
            //paddlePosition is the point in space of the centre of the paddle, while contactPoint is the first point on the paddle the ball hit
            Vector3 paddlePosition = this.transform.position;
            Vector2 contactPoint = other.GetContact(0).point;

            /*offset - the difference between the centre and the contact point,
             * width - half the width of the paddle collider, since we only need to calculate the "%" 
             * of how far the ball's contact point was from the centre of the paddle*/ 
            float offset = paddlePosition.x - contactPoint.x;
            float width = other.otherCollider.bounds.size.x / 2;

            /*gets the current angle the ball was going, calculates the angle it should bounce
             * (the "%" * max angle it can bounce, and gets the new angle the ball should go */
            if(ball != null)
            {
                float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rb.velocity);
                float bounceAngle = (offset / width) * this.maxBounceAngle;
                float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle);
                //translates the new angle into quaternions
                Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
                //applies the new angle to the ball
                ball.rb.velocity = rotation * Vector2.up * ball.rb.velocity.magnitude;
            }
            else if(extraBall != null)
            {
                float currentAngle = Vector2.SignedAngle(Vector2.up, extraBall.rb.velocity);
                float bounceAngle = (offset / width) * this.maxBounceAngle;
                float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle);
                Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
                extraBall.rb.velocity = rotation * Vector2.up * extraBall.rb.velocity.magnitude;
            }
            /*Honestly, I didn't have any idea how to do this on two different types of BallScripts
             * except separating the part of BallScript to make it behave differently if the ball name is "ExtraBall", hence the nested "if" conditions*/
        }
    }
}
