using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBallScript : MonoBehaviour
{    
    public Rigidbody2D rb { get; private set; }
    [SerializeField]
    private float _speed = 12f;
    [SerializeField]
    private float _lifeTime = 15f;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomTrajectory();
        StartCoroutine(LivingTimeCoroutine());
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();        
    }
    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * _speed;
    }
    private void SetRandomTrajectory()
    {
        //this.rb.velocity = Vector2.zero;
        Vector2 force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = 1f;
        this.rb.AddForce(force.normalized * this._speed);
    }
    
    private IEnumerator LivingTimeCoroutine(){
        yield return new WaitForSeconds(_lifeTime);
        this.gameObject.SetActive(false);
    }
}
