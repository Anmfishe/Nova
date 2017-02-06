using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthController : MonoBehaviour {

    private Animator anim; // Reference to the platform's animator component.
    bool seedActivate = false; // Keyboard input to activate the seed abilty
    bool triggerEntered = false; //Check whether Nova entered trigger for growth

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        seedActivate = Input.GetKey(KeyCode.LeftShift);

        if (seedActivate && triggerEntered)
        {
            string animname = gameObject.tag;
            Debug.Log(animname);
            anim.Play(animname, -1, 0f);
            Debug.Log("Shift hit");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered platform");
            triggerEntered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player left platform");
            anim.SetTrigger("Grow");
        }
    }
}
