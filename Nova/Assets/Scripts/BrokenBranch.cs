using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBranch : MonoBehaviour {
    public AudioClip initialBreak;
    public AudioClip branchBreak;
    private HingeJoint2D hj2d;
    private bool first = true;
    private AudioSource audioSource;
	// Use this for initialization
	void Start () {
        hj2d = GetComponent<HingeJoint2D>();
        audioSource = GetComponent<AudioSource>();        
    }
	
	// Update is called once per frame
	void Update () {
		if(hj2d == null &&  gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
        }
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && first)
        {
            first = false;
            StartCoroutine("breakBranch");
            audioSource.clip = initialBreak;
            audioSource.Play();
            
        }
    }
    IEnumerator breakBranch()
    {
        yield return new WaitForSeconds(.75f);
        audioSource.clip = branchBreak;
        audioSource.Play();
        hj2d.breakForce = -1;
    }
}
   
