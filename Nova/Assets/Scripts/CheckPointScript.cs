using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointScript : MonoBehaviour {
    private bool used = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !used)
        {
            used = true;
            other.GetComponent<CharacterController>().setRespawnPoint(transform.position);
        }
    }
}
