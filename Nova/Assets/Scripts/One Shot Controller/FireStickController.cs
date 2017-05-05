﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStickController : MonoBehaviour {
    public LayerMask whatIsGround;
    private GameObject player;
    private Transform fireTransform;
    private Collider2D c2D;
    bool canBurn = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        fireTransform = transform.Find("EmberPSTarget");
	}
	
	// Update is called once per frame
	void Update () {
        c2D = Physics2D.OverlapCircle(fireTransform.position, 0.1f, whatIsGround);
        if (c2D != null &&  c2D.tag == "Web")
        {
            player.GetComponent<CharacterController>().canBurn = true;
            player.GetComponent<CharacterController>().web = c2D.gameObject;
        }
        else
        {
            player.GetComponent<CharacterController>().canBurn = false;
        }
    }
}
