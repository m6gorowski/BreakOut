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
            if (CheckBalls())
            {
                Destroy(other.gameObject);
            }
            else
            {
                FindObjectOfType<GameManagerScript>().Miss();
            }  
        }
    }

    //Checks if it was the last ball on the scene that hit the DeadZone
    private bool CheckBalls()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        if(balls.Length > 1)
        {            
            return true;
        }
        return false;        
    }
}
