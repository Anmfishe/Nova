using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningController : MonoBehaviour {
    public bool skipOpening;
    public GameObject mainCam;
    public GameObject csController;
    public GameObject player;
    private UnitySampleAssets._2D.Camera2DFollow c2Df;
    private CharacterController cc;
    private CutsceneController csc;
    private Camera mc;
    private Camera cscc;
    // Use this for initialization
    void Start () {
        c2Df = mainCam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        cc = player.GetComponent<CharacterController>();
        csc = csController.GetComponent<CutsceneController>();
        mc = mainCam.GetComponent<Camera>();
        cscc = csController.transform.parent.GetComponent<Camera>();
        if(skipOpening)
        {
            cscc.enabled = false;
            mc.enabled = true;
            c2Df.showTitle = false;
            c2Df.startFadeIn();
            cc.canMove = true;
        }
        else
        {
            cscc.enabled = false;
            mc.enabled = true;
            cc.canMove = false;
            c2Df.showTitle = true;
            StartCoroutine(playCutsceneIn(3));
        }
	}
	IEnumerator playCutsceneIn(float time)
    {
        yield return new WaitForSeconds(time);
        c2Df.showTitle = false;
        yield return new WaitForSeconds(time);
        cscc.enabled = true;
        mc.enabled = false;
        csc.playCutScene(0);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
