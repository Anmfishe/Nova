using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableController : MonoBehaviour {
    public LayerMask ground;
    public bool beingPushed = false;
    public float dist;
    private Rigidbody2D rb2d;
    private float decel = .95f;
    public bool grounded = false;
    private float groundAngle;
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
        else if(grounded)
        {
            //rb2d.constraints = RigidbodyConstraints2D.None
            if(Mathf.Abs(rb2d.velocity.x) <= 1.5)
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX /*| RigidbodyConstraints2D.FreezeRotation*/;
            else
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x * decel, 0);
            }
           
        }
        else
        {
            
            rb2d.constraints = RigidbodyConstraints2D.None;
            //GetComponent<HingeJoint2D>().enabled = false;
            if(transform.localEulerAngles.z >= 35)
            {
                transform.localEulerAngles = Vector3.forward * 35;
            }
            else if(transform.localEulerAngles.z <= -35)
            {
                transform.localEulerAngles = Vector3.forward * -35;
            }
        }
	}
    void FixedUpdate()
    {
        //Debug.Log(LayerMask.NameToLayer("Ground"));
        Physics2D.queriesStartInColliders = false;
        Debug.DrawRay(transform.position, new Vector2(0, -dist), Color.red);
        grounded = Physics2D.Linecast(transform.position, transform.position + Vector3.down * dist, ground);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundAngle = other.gameObject.transform.localEulerAngles.z;
        }
    }
}
