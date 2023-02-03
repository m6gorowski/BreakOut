using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    public int PowerUpIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
            FindObjectOfType<GameManagerScript>().PowerUpActive(PowerUpIndex);
            Destroy(this.gameObject);
        }
    }
}
