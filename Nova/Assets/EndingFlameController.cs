using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingFlameController : MonoBehaviour {
    private SpriteRenderer sr;
    private Animator anim;
    private Transform flameCollider;
    float fadeRate = 0.01f;
    float animSpeed;
    bool first = true;
    [HideInInspector]
    public bool flameOn = false;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
        anim = GetComponent<Animator>();
        animSpeed = Random.RandomRange(1f, 1.5f);
        anim.speed = animSpeed;
        flameCollider = GameObject.FindGameObjectWithTag("FlameOn").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(flameOn)
        {
            sr.color = new Color(1, 1, 1, sr.color.a + fadeRate);
        }
        else if(flameCollider.position.y > transform.position.y)
        {
            flameOn = true;
        }
	}
    
}
