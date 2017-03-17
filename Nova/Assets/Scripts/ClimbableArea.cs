using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableArea : MonoBehaviour {
    private CharacterController playerController;
    private int numColliders;
    public int collidersRequiredToStop = 0;
	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<CharacterController>();
	}
	
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            numColliders++;
            playerController.canClimb = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            numColliders--;
            if(numColliders <= collidersRequiredToStop)
            playerController.canClimb = false;
        }
    }
}
