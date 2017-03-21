using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraAreaScript : MonoBehaviour {
    //Public References//
    public float targetSize; // What size will the zoom be in this area 
    public bool keepNovaAsTarget = false;
    public bool useOnce = true;
    public float aboveOffset = 0;
    public float t_rate = 0.01f;
    public bool freezeNova = false;
    public bool timed = false;
    public float duration;
    public bool changeDamping = false;
    public float damp;
    //Private References//
    private UnitySampleAssets._2D.Camera2DFollow c2Df; // The main camera's script
    private float camSizeSave; // The original size of the camera
    private float camSizeSave2; // The size of the camera when it started to shrink, NOT ALWAYS THE TARGET SIZE
    private bool playerIn; // Is the player in the space
    private bool active = false;
    private bool used = false;
    private float t = 0; // This is a keeper for lerping
     // This is the time step for lerping
    private Camera cam; // Get the main camera
    private int numColliders = 0;
    private GameObject player;
    private float dampingSave = 0.3f;




    // Use this for initialization and setting up references
    void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        c2Df = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        //camSizeSave = cam.orthographicSize;
	}
	

    //Update the camera's zoom depending on if Nova is in the area
	void FixedUpdate () {
        if (playerIn && (cam.orthographicSize != targetSize))
        {
            cam.orthographicSize = Mathf.Lerp(camSizeSave, targetSize, t);
            t += t_rate;
        }
        else if (!playerIn && (cam.orthographicSize != camSizeSave) && active)
        {
            cam.orthographicSize = Mathf.Lerp(camSizeSave2, camSizeSave, t);
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
        if (other.gameObject.tag == "Player" && !used)
        {
            player = other.gameObject;
            numColliders++;
            if(numColliders == 2 && freezeNova && !used)
            {
                other.GetComponent<CharacterController>().canMove = false;
            }
            if (useOnce && !used && numColliders == 2)
            {
                used = true;
            }
            if (!keepNovaAsTarget)
            {
                c2Df.posFixed = true;
                c2Df.target = transform;
            }
            camSizeSave = cam.orthographicSize;
            playerIn = true;
            active = true;
            t = 0;
            if(changeDamping)
            {
                
                
                c2Df.damping = damp;
            }
            if(timed)
            {
                StartCoroutine(wait());
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        
        if (other.gameObject.tag == "Player")
        {
            numColliders--;
            if (numColliders <= 1 && !timed)
            {
                camSizeSave2 = cam.orthographicSize;
                playerIn = false;
                c2Df.posFixed = false;
                c2Df.target = other.transform;
                t = 0;
            }
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(duration);
        exitFixed();
    }
    void exitFixed()
    {
        if (freezeNova)
        {
            player.GetComponent<CharacterController>().canMove = true;
        }
        c2Df.damping = dampingSave;
        camSizeSave2 = cam.orthographicSize;
        playerIn = false;
        c2Df.posFixed = false;
        c2Df.target = player.transform;
        t = 0;
    }
}
