﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRegrowth : MonoBehaviour {
    public Sprite e;
    public GameObject brokenBranch;
    private HingeJoint2D hj2d;
    private GameObject player;
    private SpriteRenderer sr;
    private float currentAlpha = 0;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        sr.material.color = new Color(1f, 1f, 1f, currentAlpha);
        sr.sprite = e;
        hj2d = brokenBranch.GetComponent<HingeJoint2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (player.GetComponent<CharacterController>().canGrow() && !player.GetComponent<CharacterController>().getHasGrown() 
            && player.GetComponent<CharacterController>().getDir() && hj2d == null)
        {
            if(currentAlpha < 1)
            {
                currentAlpha += 0.01f;
            }
        }
        else
        {
            if(currentAlpha > 0)
            {
                currentAlpha -= 0.01f;
            }
        }
        sr.material.color = new Color(1f, 1f, 1f, currentAlpha);
    }
}
