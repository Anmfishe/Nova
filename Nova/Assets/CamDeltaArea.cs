using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDeltaArea : MonoBehaviour {
    public Camera cam;
    public float deltaRate;
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
            cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().shiftCamToNova();
            if(prevX < player.transform.position.x)
            {
                cam.orthographicSize += deltaRate;
            }
            else if(prevX > player.transform.position.x)
            {
                if (!(cam.orthographicSize < 10))
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
