using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderTreeEndingv2 : MonoBehaviour {
    private CharacterController cc;
    private FireNovaController fnc;
    private Camera cam;
	// Use this for initialization
	void Start () {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        fnc = GameObject.FindGameObjectWithTag("FireLady").GetComponent<FireNovaController>();
        cam = Camera.main;
        StartCoroutine(Opening());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Opening()
    {
        cc.canMove = false;
        cc.Flip();
        cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeIn();
        cc.anim.Play("NovaSitToStand");
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        cc.canMove = true;

    }
}
