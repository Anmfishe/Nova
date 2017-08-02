using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaLandingOnFace : MonoBehaviour {

    private CharacterController cc;
    bool first = true;
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
            first = false;
            StartCoroutine(FaceLanding());
        }
    }

    IEnumerator FaceLanding()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(0.4f);
        cc.anim.SetBool("Landing", true);
        yield return new WaitForSeconds(2f);
        cc.canMove = true;
        cc.anim.SetBool("Landing", false);
    }
}
