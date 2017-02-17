using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToMeScript : MonoBehaviour {

    // Use this for initialization
    private Transform savedParent;
    private int numColliders;
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Nova-pieces")
        {
            other.transform.parent = transform.parent;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Nova-pieces")
        {
            other.transform.parent = null;
        }
    }
}
