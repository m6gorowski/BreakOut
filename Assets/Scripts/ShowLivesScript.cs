using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLivesScript : MonoBehaviour
{
    public Transform paddleTransform { get; private set; }
    public Color _aliveColor;
    public Color _deadColor;

    private void Awake()
    {
        paddleTransform = FindObjectOfType<PaddleScript>().transform;
    }
    private void Update()
    {
        transform.position = new Vector3(paddleTransform.position.x, transform.position.y, transform.position.z);
    }
}
