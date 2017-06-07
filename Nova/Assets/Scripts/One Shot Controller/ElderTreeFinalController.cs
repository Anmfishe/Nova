﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderTreeFinalController : MonoBehaviour {
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    private Transform target;
    private FixedCameraAreaScript fca;
    private CharacterController cc;
	// Use this for initialization
	void Start () {
        c2DF = Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        target = transform.Find("OpeningTarget");
        fca = GetComponent<FixedCameraAreaScript>();
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        StartCoroutine(Opening());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Opening()
    {
        cc.canMove = false;
        c2DF.target = target;
        c2DF.posFixed = true;
        c2DF.whiteFadeIn();
        fca.setCamSize(40, 0.001f);
        yield return new WaitForSeconds(13);
        c2DF.startFadeOut();
        yield return new WaitForSeconds(8);
        Application.LoadLevel("Intro");
    }
}
