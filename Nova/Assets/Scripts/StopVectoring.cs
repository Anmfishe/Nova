using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVectoring : MonoBehaviour {
    private GameObject player;
    private CharacterController cc;
    int numColliders = 0;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        numColliders++;
        if (other.gameObject.tag == "Player")
        {
            cc.setVector(false);
        }
        
    }
    void OnCollisionExit2D(Collision2D other)
    {
        numColliders--;
        if (other.gameObject.tag == "Player" && numColliders == 0)
            cc.setVector(true);
    }
}
