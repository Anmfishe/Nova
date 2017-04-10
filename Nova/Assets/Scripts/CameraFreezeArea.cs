using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreezeArea : MonoBehaviour {
    private Camera cam;
    private GameObject player;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            c2DF.stopMoving = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            c2DF.stopMoving = false;
        }
        
    }
}
