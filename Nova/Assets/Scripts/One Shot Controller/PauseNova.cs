using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseNova : MonoBehaviour
{
    private bool first = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && first)
        {
            other.GetComponent<CharacterController>().pauseNova = true;
            first = false;
        }
    }
}
