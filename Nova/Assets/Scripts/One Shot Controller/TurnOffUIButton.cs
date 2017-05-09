using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffUIButton : MonoBehaviour {
    private CharacterController cc;
	// Use this for initialization
	void Start () {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            cc.hasPushed = true;
            Destroy(gameObject);
        }
    }
}
