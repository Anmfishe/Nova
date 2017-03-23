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
    void OnCollisionStay2D(Collision2D other)
    {
        //numColliders++;
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            cc.setVector(false);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            cc.setVector(false);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -200));
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        numColliders--;
        if (other.gameObject.tag == "Player" /*&& numColliders == 0*/) {
            StartCoroutine(resetVector());
        }
            //cc.setVector(true);
    }
    IEnumerator resetVector()
    {
        yield return new WaitForSeconds(0.5f);
        cc.setVector(true);

        
    }
}
