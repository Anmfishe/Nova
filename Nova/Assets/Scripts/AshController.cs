using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshController : MonoBehaviour {
    private Rigidbody2D rb2d;
    private bool grounded = false;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //grounded = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.NameToLayer("Ground"));

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Something hit " + LayerMask.LayerToName(collision.gameObject.layer) + " " + Time.time);
        if (collision.gameObject.layer == LayerMask.NameToLayer("NovaBody") && grounded)
        {
            Debug.Log("Hit Nova " + Time.time);
            grounded = false;
            float x = Random.Range(-1, 1);
            float y = Random.Range(-1, 4);
            rb2d.AddForce(new Vector2(x, y));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Ground hit " + Time.time);
            grounded = true;
        }
    }
}
