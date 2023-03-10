using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneScript : MonoBehaviour
{
    //In the gameManager, if the Ball hits the DeadZone, which is a place where player's ball "dies"
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");            
            Destroy(other.gameObject);
            if (balls.Length <= 1)
            {
                FindObjectOfType<GameManagerScript>().Miss();
            }
        }
    }
}
