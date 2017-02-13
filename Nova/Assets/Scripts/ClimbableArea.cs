using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableArea : MonoBehaviour {
    private CharacterController playerController;
    private int numColliders;
	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<CharacterController>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Nova-pieces")
        {
            numColliders++;
            playerController.canClimb = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Nova-pieces")
        {
            numColliders--;
            if(numColliders == 0)
            playerController.canClimb = false;
        }
    }
}
