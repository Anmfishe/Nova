using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNovaStopper : MonoBehaviour {

    private CharacterController cc;
    bool second = true;

    // Use this for initialization
    void Start () {
        cc = GameObject.FindGameObjectWithTag("FireLady").GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FireLady")
        {
            second = false;
            StartCoroutine(FireStopping());
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("FireLady") && !GetComponent<EdgeCollider2D>().isTrigger)
        {
            GetComponent<EdgeCollider2D>().isTrigger = false;
        }
    }

   /* IEnumerator FireStopping()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(0.4f);
        cc.anim.SetBool("FireNovaLevel3Idle", true);
       /* yield return new WaitForSeconds(2f);
        cc.canMove = true;
        cc.anim.SetBool("FireNovaLevel3Idle", false);
    }*/
}
