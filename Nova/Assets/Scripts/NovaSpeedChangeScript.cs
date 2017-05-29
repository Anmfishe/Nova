using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSpeedChangeScript : MonoBehaviour {
    private GameObject player;
    private bool playerIn = false;
    private int numColliders = 0;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerIn)
        {
            if (player.GetComponent<CharacterController>().getDir())
            {
                player.GetComponent<CharacterController>().anim.SetBool("Wind", true);
                player.GetComponent<CharacterController>().speedCoef = 0.5f;
                player.transform.localEulerAngles = new Vector3(0, 0, 10);
            }
            else
            {
                player.GetComponent<CharacterController>().anim.SetBool("Wind", false);
                player.GetComponent<CharacterController>().speedCoef = 1.3f;
                player.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            numColliders++;
            if (numColliders == 1)
            {
                other.transform.localEulerAngles = new Vector3(0, 0, 10);
                playerIn = true;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            numColliders--;
            if (numColliders == 0)
            {
                //Debug.Log(gameObject + " " + Time.time);
                playerIn = false;
                other.transform.localEulerAngles = new Vector3(0, 0, 0);
                player.GetComponent<CharacterController>().speedCoef = 1f;
                player.GetComponent<CharacterController>().anim.SetBool("Wind", false);

            }
        }
    }
}
