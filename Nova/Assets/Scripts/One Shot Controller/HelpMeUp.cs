using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMeUp : MonoBehaviour {
    private CharacterController cc;
    private FireNovaController fnc;
    private Camera cam;
    bool first = true;
    // Use this for initialization
    void Start()
    {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        fnc = GameObject.FindGameObjectWithTag("FireLady").GetComponent<FireNovaController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && first)
        {
            first = false;
        }
    }
    IEnumerator HelpMeUpRoutine()
    {
        yield return new WaitForSeconds(1f);
        //Do like a bunch of stuff

    }
}
