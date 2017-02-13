using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBranch : MonoBehaviour {
    private HingeJoint2D hj2d;
    
	// Use this for initialization
	void Start () {
        hj2d = GetComponent<HingeJoint2D>();
        
    }
	
	// Update is called once per frame
	void Update () {
		if(hj2d == null &&  gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
        }
	}
    /*void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            landed = true;
        }
    }*/
}
   
