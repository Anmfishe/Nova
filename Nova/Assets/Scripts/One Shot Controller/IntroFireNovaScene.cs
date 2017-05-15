using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroFireNovaScene : MonoBehaviour {
    private GameObject player;
    private GameObject fireNova;
    private CharacterController cc;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    bool first = true;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        fireNova = GameObject.FindGameObjectWithTag("FireLady");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && first)
        {
            first = false;
            StartCoroutine(IntroScene());
        }
    }
    IEnumerator IntroScene()
    {
        yield return new WaitForSeconds(1.5f);
        cc.anim.Play("NovaIntroScene2End");
        yield return new WaitForSeconds(2.25f);
        fireNova.GetComponent<FireNovaController>().moveFN(0.05f);
    }
}
