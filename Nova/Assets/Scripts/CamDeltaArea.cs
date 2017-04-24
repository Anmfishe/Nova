using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDeltaArea : MonoBehaviour {
    public Camera cam;
    public float deltaRate;
    public float maxSize = 40;
    public float minSize = 10;
    public bool shiftCamToNova = true;
    private float prevX;
    private GameObject player;
    private CharacterController cc;
    private bool playerIn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(playerIn)
        {
            if(shiftCamToNova)
            cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().shiftCamToNova();
            if(prevX < player.transform.position.x)
            {
                if (!(cam.orthographicSize >= maxSize))
                    cam.orthographicSize += deltaRate;
            }
            else if(prevX > player.transform.position.x)
            {
                if (!(cam.orthographicSize <= minSize))
                    cam.orthographicSize -= deltaRate;
            }
            prevX = player.transform.position.x;
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIn = true;
            player = other.gameObject;
            prevX = player.transform.position.x;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerIn = false;
            //player = other.gameObject;
        }
    }
}
