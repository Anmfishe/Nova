using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2S1OpeningController : MonoBehaviour {
    private GameObject player;
    private Camera cam;
    private CharacterController cc;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    bool first = true;
    private Transform altarTarget;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        altarTarget = transform.Find("AltarTarget");
        c2DF.startFadeIn();
        cc.anim.Play("NovaSitToStand");
        cc.hasPushed = true;
        StartCoroutine(stopNova(2));
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && first)
        {
            first = false;
            StartCoroutine(StartIntro());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
    IEnumerator StartIntro()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(0.5f);
        c2DF.damping = 1f;
        c2DF.target = altarTarget;
        c2DF.posFixed = true;
        yield return new WaitForSeconds(5);
        c2DF.damping = 0.3f;
        c2DF.target = player.transform;
        c2DF.posFixed = false;
        yield return new WaitForSeconds(1.25f);
        cc.canMove = true;
    }
    IEnumerator stopNova(float time)
    {
        cc.canMove = false;
        yield return new WaitForSeconds(time);
        cc.canMove = true;
    }
}

