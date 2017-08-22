using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaFallOnFace : MonoBehaviour {
    public FixedCameraAreaScript fca;
    private CharacterController cc;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    bool first = true;
	// Use this for initialization
	void Start () {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        c2DF = Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(first && collision.tag == "Player")
        {
            first = false;
            StartCoroutine(FallOnFace());
        }
    }
    IEnumerator FallOnFace()
    {
        cc.canMove = false;
        c2DF.posFixed = true;
        yield return new WaitForSeconds(0.5f);
        c2DF.damping = 0.05f;
        //Play some anim 
        yield return new WaitForSeconds(1f);
        fca.setCamSize(6.5f, 0.01f);
        yield return new WaitForSeconds(4f);
        c2DF.posFixed = false;
        c2DF.damping = 0.3f;
        fca.setCamSize(10, 0.005f);
        cc.canMove = true;

    }
}
