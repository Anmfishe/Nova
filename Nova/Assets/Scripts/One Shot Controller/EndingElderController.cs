using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingElderController : MonoBehaviour {
    public GameObject[] burningParts;
    public GameObject fireDeath;
    private GameObject player;
    private GameObject fireNova;
    private CharacterController cc;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        fireNova = GameObject.FindGameObjectWithTag("FireLady");
        StartCoroutine(OpeningScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator OpeningScene()
    {
        cc.canMove = false;
        cc.anim.Play("NovaSitToStand");
        c2DF.startFadeIn();
        yield return new WaitForSeconds(2.5f);
        cc.canMove = true;
        StartCoroutine(BurnRoutine());

    }
    IEnumerator BurnRoutine()
    {
        yield return new WaitForSeconds(2);
        burningParts[0].GetComponent<BurnTree>().Burn();
        yield return new WaitForSeconds(5);
        burningParts[1].GetComponent<BurnTree>().Burn();
        yield return new WaitForSeconds(2.5f);
        fireDeath.GetComponent<MoveFireUp>().move = true;
        yield return new WaitForSeconds(2.5f);
        burningParts[2].GetComponent<BurnTree>().Burn();
        burningParts[3].GetComponent<BurnTree>().Burn();
    }
}
