using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableController : MonoBehaviour {
    public bool beingPushed = false;
    private Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(beingPushed)
        {
            //rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb2d.constraints = RigidbodyConstraints2D.None;
            
        }
        else
        {
            //rb2d.constraints = RigidbodyConstraints2D.None
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            
        }
	}
    void FixedUpdate()
    {

    }
}
