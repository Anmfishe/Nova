using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthController : MonoBehaviour {

    private Animator anim; // Reference to the platform's animator component.
	
    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            string animname = gameObject.tag;
            Debug.Log(animname);
            anim.Play(animname, -1, 0f);
            Debug.Log("Player entered platform");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player left platform");
            anim.SetTrigger("Grow");
        }
    }
}
