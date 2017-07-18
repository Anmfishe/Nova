using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3OpeningController : MonoBehaviour {
    private GameObject player;
    private CharacterController cc;
    Camera cam;
    UnitySampleAssets._2D.Camera2DFollow c2DF;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        cc.canMove = false;
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        c2DF.startFadeIn();
        cc.anim.Play("NovaSitToStand");
        StartCoroutine(opening());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator opening()
    {
        yield return new WaitForSeconds(2f);
        cc.canMove = true;
    }
}
