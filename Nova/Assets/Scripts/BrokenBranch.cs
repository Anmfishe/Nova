using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBranch : MonoBehaviour {
    public AudioClip initialBreak;
    public AudioClip branchBreak;
    public float breakTime = 0.75f;
    public float angleLimit = 10;
    public bool canMoveOff = false;
    public bool regrow = false;
    public bool freezeNova = false;
    private Transform regrowthObj; 
    private HingeJoint2D hj2d;
    private bool first = true;
    private int timesCollided = 0;
    private AudioSource audioSource;
    private Rigidbody2D rb2d;
    private GameObject player;
	// Use this for initialization
	void Start () {
        hj2d = GetComponent<HingeJoint2D>();
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
		if(hj2d == null &&  gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            gameObject.layer = LayerMask.NameToLayer("NoNovaCollision");
        }
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && first)
        {
            timesCollided++;
            JointAngleLimits2D jal2d = new JointAngleLimits2D();
            jal2d.min = 0;
            jal2d.max = angleLimit;
            hj2d.limits = jal2d;
            first = false;
            StartCoroutine("breakBranch");
            audioSource.clip = initialBreak;
            audioSource.Play();
            if(freezeNova)
            {
               player.GetComponent<CharacterController>().canMove = false;
            }
            
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !first && canMoveOff && timesCollided < 5)
        {
           //Debug.Log(timesCollided);
            first = true;
            StopCoroutine("breakBranch");
            //audioSource.clip = initialBreak;
            //audioSource.Play();

        }
    }
    IEnumerator breakBranch()
    {
        yield return new WaitForSeconds(breakTime);
        audioSource.clip = branchBreak;
        audioSource.Play();
        hj2d.breakForce = -1;
        rb2d.isKinematic = false;
        player.GetComponent<CharacterController>().canMove = true;
        //GetComponent<BoxCollider2D>().enabled = false;
        //GetComponent<PolygonCollider2D>().enabled = true;

    }
}
   
