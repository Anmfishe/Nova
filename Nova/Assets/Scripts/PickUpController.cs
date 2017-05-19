using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private Collider2D c2d;
    private float zRotSave;
    private Vector3 scaleSave;
    [HideInInspector]
    public bool beingHeld = false;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        c2d = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(beingHeld)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.parent.localEulerAngles.z * -1 - 25);
        }
	}
    public void pickUp()
    {
        scaleSave = transform.localScale;
        beingHeld = true;
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = true;
        c2d.enabled = false;
        transform.localEulerAngles = Vector3.zero;
    }
    public void drop()
    {
        beingHeld = false;
        rb2d.isKinematic = false;
        rb2d.constraints = RigidbodyConstraints2D.None;
        c2d.enabled = true;
        transform.localScale = Vector3.one;
        gameObject.layer = LayerMask.NameToLayer("NoNovaCollision");
        transform.localScale = scaleSave;
    }
}
