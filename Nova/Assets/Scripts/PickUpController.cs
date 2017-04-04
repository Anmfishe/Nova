using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Collider2D c2d;
    [HideInInspector]
    public bool beingHeld = false;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(beingHeld)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.parent.localEulerAngles.z * -1);
        }
	}
    public void pickUp()
    {
        beingHeld = true;
        rb2d.isKinematic = true;
        c2d.enabled = false;
    }
    public void drop()
    {
        beingHeld = false;
        rb2d.isKinematic = false;
        c2d.enabled = true;
    }
}
