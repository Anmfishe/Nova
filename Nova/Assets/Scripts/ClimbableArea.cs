using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableArea : MonoBehaviour {
    private GameObject player;
    private CharacterController playerController;
    private int numColliders;
    public int collidersRequiredToStop = 0;
    public bool requiresRight = false;
    public bool requiresLeft = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = FindObjectOfType<CharacterController>();
	}
    
    
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            numColliders++;
            if ((requiresRight && playerController.getDir())  
                || (!requiresRight && !requiresLeft) 
                || (requiresLeft && !playerController.getDir()))
            {
                
                playerController.canClimb = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if((requiresRight && !(player.transform.position.x < (transform.position.x - 0.25)))
                || (requiresLeft && !(player.transform.position.x > (transform.position.x + 0.25))))
            {
                playerController.canClimb = false;
            }
            
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
