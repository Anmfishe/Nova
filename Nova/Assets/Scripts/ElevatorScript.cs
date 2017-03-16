using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour {
    private GameObject player;
    public Transform target;
    public float lerpRate = 0.005f;
    public bool left = false;
    private float lerpOne = 0;
    private CharacterController cc;
    private Vector3 origPos;
    bool ready = true;
    bool playerInArea = false;
    bool lerping = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(playerInArea && ((cc.getDir() && !left) || (!cc.getDir() && left)) && !lerping)
        {
            cc.toggleElevating();
            origPos = player.transform.position;
            lerping = true;
        }
        else if(lerping && lerpOne < 1)
        {
            Vector3 pos;
            pos.z = origPos.z;
            pos.x = Mathf.Lerp(origPos.x, target.position.x, lerpOne);
            pos.y = Mathf.Lerp(origPos.y, target.position.y, lerpOne);
            player.transform.position = pos;
            lerpOne += lerpRate;
        }
        else if(lerpOne >= 1)
        {
            
            cc.toggleElevating();
            lerping = false;
            playerInArea = false;
            ready = true;
            lerpOne = 0;    
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && ready)
        {
            playerInArea = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
}
