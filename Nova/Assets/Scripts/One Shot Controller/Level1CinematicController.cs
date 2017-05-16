using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1CinematicController : MonoBehaviour {
    public GameObject branch;
    public GameObject FCA;
    public AudioClip novaScare;
    public AudioSource burnySource;
    private GameObject player;
    private GameObject fireLady;
    private CharacterController cc;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    private AudioSource audioSource;
    public Transform target;
    bool first = true;
    bool lowerMusic = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        fireLady = GameObject.FindGameObjectWithTag("FireLady");
        audioSource = GetComponent<AudioSource>();
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        //target = transform.Find("Target");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(lowerMusic)
        {
            audioSource.volume -= 0.02f;
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && first)
        {
            first = false;
            StartCoroutine(CinematicController());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            lowerMusic = false;
        }
    }
    IEnumerator CinematicController()
    {
        cc.canMove = false;
        lowerMusic = true;
        yield return new WaitForSeconds(0.5f);
        FCA.SetActive(true);
        FCA.GetComponent<FixedCameraAreaScript>().setCamSize(10, 0.04f);
        c2DF.target = target;
        c2DF.posFixed = true;
        yield return new WaitForSeconds(0.25f);
        fireLady.GetComponent<Animator>().Play("FireNovaLevel1");
        player.GetComponent<Animator>().Play("Level1Scene3Cinematic");
        yield return new WaitForSeconds(0.25f);
        FCA.SetActive(false);
        audioSource.Stop();
        lowerMusic = false;
        audioSource.volume = 1;
        audioSource.clip = novaScare;
        audioSource.PlayOneShot(novaScare, 1);
        yield return new WaitForSeconds(0.25f);
        burnySource.Play();
        yield return new WaitForSeconds(0.5f);
        branch.GetComponent<BurningBranchController>().BurnIt();
        yield return new WaitForSeconds(2.5f);
        yield return new WaitForSeconds(2.0f);
        
        branch.GetComponent<AudioSource>().Play();
        branch.GetComponent<HingeJoint2D>().breakForce = -1;
        branch.GetComponent<Collider2D>().enabled = false;
        //branch.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
        yield return new WaitForSeconds(2f);
        c2DF.posFixed = false;
        c2DF.target = player.transform;
        yield return new WaitForSeconds(6f);
        cc.canMove = true;
        

    }
}
