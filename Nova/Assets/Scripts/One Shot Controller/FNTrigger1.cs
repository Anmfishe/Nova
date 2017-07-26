using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FNTrigger1 : MonoBehaviour {
    private GameObject fireNova;
    bool first = true;
	// Use this for initialization
	void Start () {
        fireNova = GameObject.FindGameObjectWithTag("FireLady");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && first)
        {
            first = false;
            //Do some shiz with FN
        }
    }
}
