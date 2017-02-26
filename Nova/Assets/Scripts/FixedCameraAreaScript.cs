using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraAreaScript : MonoBehaviour {
    //Public References//
    public float targetSize; // What size will the zoom be in this area 

    //Private References//
    private UnitySampleAssets._2D.Camera2DFollow c2Df; // The main camera's script
    private float camSizeSave; // The original size of the camera
    private bool playerIn; // Is the player in the space
    private bool active = false;
    private float t = 0; // This is a keeper for lerping
    private float t_rate = 0.01f; // This is the time step for lerping
    private Camera cam; // Get the main camera



    // Use this for initialization and setting up references
    void Start () {
        cam = Camera.main;
        c2Df = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        camSizeSave = cam.orthographicSize;
	}
	

    //Update the camera's zoom depending on if Nova is in the area
	void FixedUpdate () {
        if (playerIn && (cam.orthographicSize < targetSize))
        {
            cam.orthographicSize = Mathf.Lerp(camSizeSave, targetSize, t);
            t += t_rate;
        }
        else if (!playerIn && (cam.orthographicSize > camSizeSave) && active)
        {
            cam.orthographicSize = Mathf.Lerp(targetSize, camSizeSave, t);
            t += t_rate;
        }
        else if(!playerIn && cam.orthographicSize == camSizeSave && active)
        {
            active = false;
        }
	}



    //Set bools and timer values if Nova has entered or exited the area
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            c2Df.target = transform;
            playerIn = true;
            active = true;
            t = 0;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIn = false;
            c2Df.target = other.transform;
            t = 0;
        }
    }
}
