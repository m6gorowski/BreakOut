using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    public int PowerUpIndex = 0;
    public AudioManagerScript AudioManagerScript { get; private set; }

    private void Awake()
    {
        this.AudioManagerScript = GameObject.FindObjectOfType<AudioManagerScript>();
    }

    void Update()
    {
        transform.Translate(_speed * Vector3.down * Time.deltaTime);
        if(transform.position.y < -20f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Paddle")
        {
            AudioManagerScript.PlaySFX(AudioManagerScript.powerUpHit);
            FindObjectOfType<GameManagerScript>().PowerUpActive(PowerUpIndex);
            Destroy(this.gameObject);
        }
    }
}
