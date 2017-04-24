using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour {
    private bool startflapping;
    private bool flapready = true;
    private Animator b_anim;
	// Use this for initialization
	void Start () {
        b_anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(startflapping && flapready)
        {
            flapready = false;
            b_anim.Play("FlapAnim");
            StartCoroutine(flapCD(Random.Range(1.6f, 3f)));
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            startflapping = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            startflapping = false;
        }
    }
    IEnumerator flapCD(float time)
    {
        yield return new WaitForSeconds(time);
        flapready = true;
    }
}
