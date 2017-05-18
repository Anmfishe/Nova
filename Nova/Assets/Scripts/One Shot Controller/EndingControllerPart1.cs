﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingControllerPart1 : MonoBehaviour
{
    public GameObject[] burningParts;
    public GameObject fireDeath;
    public BrokenBranch bb;
    private GameObject player;
    private GameObject fireNova;
    private CharacterController cc;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    private Transform target;
    bool first = true;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        fireNova = GameObject.FindGameObjectWithTag("FireLady");
        target = transform.Find("OpeningTarget");
        StartCoroutine(OpeningScene());

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player" && first)
        {
            first = false;
            StartCoroutine(breakIt());
        }
    }
    IEnumerator breakIt()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(0.25f);
        bb.breakIt();
        yield return new WaitForSeconds(3);
        c2DF.target = target;
        c2DF.posFixed = true;
        yield return new WaitForSeconds(0.5f);
        fireNova.GetComponent<FireNovaController>().anim.Play("FireNovaElderTreeEnd");
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(BurnRoutine());
        yield return new WaitForSeconds(3);
        c2DF.target = player.transform;
        c2DF.posFixed = false;
        cc.canMove = true;
    }
    IEnumerator OpeningScene()
    {
        cc.canMove = false;
        cc.anim.Play("NovaSitToStand");
        c2DF.startFadeIn();
        yield return new WaitForSeconds(2.5f);
        cc.canMove = true;
        //c2DF.target = target;
        //c2DF.posFixed = true;
        //yield return new WaitForSeconds(0.5f);
        //fireNova.GetComponent<FireNovaController>().anim.Play("FireNovaElderTreeEnd");
        //yield return new WaitForSeconds(3.5f);
        //StartCoroutine(BurnRoutine());
        //yield return new WaitForSeconds(3);
        //c2DF.target = player.transform;
        //c2DF.posFixed = false;
        //cc.canMove = true;

    }
    IEnumerator BurnRoutine()
    {
        yield return new WaitForSeconds(2);
        burningParts[0].GetComponent<BurnTree>().Burn();
        burningParts[1].GetComponent<BurnTree>().Burn();
        yield return new WaitForSeconds(5);
        burningParts[2].GetComponent<BurnTree>().Burn();
        burningParts[3].GetComponent<BurnTree>().Burn();
        yield return new WaitForSeconds(2.5f);
        fireDeath.GetComponent<MoveFireUp>().move = true;
        yield return new WaitForSeconds(2.5f);
        burningParts[4].GetComponent<BurnTree>().Burn();
        burningParts[5].GetComponent<BurnTree>().Burn();
    }

}
