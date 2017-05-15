using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNovaController : MonoBehaviour {
    private ParticleSystem ps;
    private float dx = 0;
    [HideInInspector]
    public Animator anim;
    private bool facingRight = false;
	// Use this for initialization
	void Start () {
        ps = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(dx > 0 && !facingRight)
        {
            Flip();
        }
        else if(dx < 0 && facingRight)
        {
            Flip();
        }
        anim.SetFloat("Speed", dx);
        Vector3 t = transform.position;
        t.x += dx;
        transform.position = t;
        
	}
    public void togglePS(int i)
    {
        if(i % 2 == 0)
        {
            ps.Play();
        }
        else
        {
            ps.Stop();
        }
    }
    public void moveFN(float deltaX)
    {
        dx = deltaX;
    }
    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        
    }
}
