using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1CinematicController : MonoBehaviour {
    public GameObject branch;
    public GameObject FCA;
    private GameObject player;
    private GameObject fireLady;
    private CharacterController cc;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    bool first = true;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        fireLady = GameObject.FindGameObjectWithTag("FireLady");
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && first)
        {
            first = false;
            StartCoroutine(CinematicController());
        }
    }
    IEnumerator CinematicController()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(0.5f);
        FCA.SetActive(true);
        FCA.GetComponent<FixedCameraAreaScript>().setCamSize(10, 0.01f);
        yield return new WaitForSeconds(0.25f);
        fireLady.GetComponent<Animator>().Play("OpenEyes");
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Animator>().Play("Level1Scene3Cinematic");
        yield return new WaitForSeconds(0.5f);
        //burn the branch
        yield return new WaitForSeconds(5f);
        branch.GetComponent<HingeJoint2D>().breakForce = -1;
        branch.layer = LayerMask.NameToLayer("NoNovaCollision");
        yield return new WaitForSeconds(6.5f);
        cc.canMove = true;
        

    }
}
