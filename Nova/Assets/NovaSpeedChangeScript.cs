using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSpeedChangeScript : MonoBehaviour {
    private GameObject player;
    private bool playerIn = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (playerIn)
        {
            if (player.GetComponent<CharacterController>().getDir())
            {
                player.GetComponent<CharacterController>().anim.SetBool("Wind", true);
                player.GetComponent<CharacterController>().speedCoef = 0.5f;

            }
            else
            {
                player.GetComponent<CharacterController>().anim.SetBool("Wind", false);
                player.GetComponent<CharacterController>().speedCoef = 1.3f;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            playerIn = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            playerIn = false;
            player.GetComponent<CharacterController>().speedCoef = 1f;
            player.GetComponent<CharacterController>().anim.SetBool("Wind", false);
        }
    }
}
