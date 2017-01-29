using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableArea : MonoBehaviour {
    private CharacterController playerController;
	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<CharacterController>();
	}
	
	void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Nova-pieces")
        {
            playerController.canClimb = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Nova-pieces")
        {
            playerController.canClimb = false;
        }
    }
}
