using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBranch : MonoBehaviour {
    private HingeJoint2D hj2d;
    private BoxCollider2D bc2d;
    private Rigidbody2D rb2d;
    private bool landed = false;
	// Use this for initialization
	void Start () {
        hj2d = GetComponent<HingeJoint2D>();
        bc2d = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
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
   
