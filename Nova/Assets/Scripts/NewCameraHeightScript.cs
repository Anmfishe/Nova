using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCameraHeightScript : MonoBehaviour {
    public float heightChange;
    public bool resetCamPos;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2Df;
    private bool used = false;
    // Use this for initialization
    void Start () {
        cam = Camera.main;
        c2Df = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !used)
        {
            //used = true;
            c2Df.shiftCamToNova();
        }
    }
}
