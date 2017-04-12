using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSpeedChangeScript : MonoBehaviour {
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        player.GetComponent<CharacterController>().speedCoef = 0.5f;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        player.GetComponent<CharacterController>().speedCoef = 1f;
    }
}
