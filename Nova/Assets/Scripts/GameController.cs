using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool buildVersion = false;
    public string[] levelsToLoad;
    private AudioSource ads;
    private bool fadeMusic = false;
    private GameObject player;
    private CharacterController cc;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        ads = GetComponent<AudioSource>();
        if (buildVersion)
        {
            foreach(string s in levelsToLoad)
            Application.LoadLevelAdditive(s);
        }
        StartCoroutine(Opening());
    }
    IEnumerator Opening()
    {
        cc.canMove = false;
        cc.anim.Play("NovaSitToStand");
        c2DF.startFadeIn();
        yield return new WaitForSeconds(2.5f);
        cc.canMove = true;
    }
}

    // Update is called once per frame
    
