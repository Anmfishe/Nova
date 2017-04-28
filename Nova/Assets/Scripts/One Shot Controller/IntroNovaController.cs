using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNovaController : MonoBehaviour {
    private GameObject player;
    private bool moveNova = false;
    private int numColliders = 0;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (moveNova)
        {
            player.transform.position = new Vector3(player.transform.position.x + 0.06f, player.transform.position.y, player.transform.position.z);
            
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            numColliders++;
            if (numColliders == 1)
            {
                player.GetComponent<Animator>().Play("NovaRun 1");
                
                StartCoroutine(moveNovaAfterSecond(6));
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
    IEnumerator moveNovaAfterSecond(float time)
    {
        yield return new WaitForSeconds(time);
        //player.GetComponent<CharacterController>().Move(1, 0, false, false);
        
        //player.GetComponent<Animator>().Play("NovaRun 1");
        yield return new WaitForSeconds(2);
    }
}
