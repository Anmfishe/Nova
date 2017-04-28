using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderTreeLevel2Controller : MonoBehaviour {
    public GameObject player;
    public Camera cam;
    private CharacterController cc;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
	// Use this for initialization
	void Start () {
        cc = player.GetComponent<CharacterController>();
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        StartCoroutine(Opening());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Opening()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(1);
        c2DF.startFadeIn();
        yield return new WaitForSeconds(2);
        cc.canMove = true;
    }
}
