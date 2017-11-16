using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaFallOnFace2 : MonoBehaviour {
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
            StartCoroutine(FallOnFace2());
        }
    }
    IEnumerator FallOnFace2()
    {
        cc.anim.SetBool("FallOnFace", true);
        fca.setCamSize(6.5f, 0.005f);
        yield return new WaitForSeconds(1f);
        cc.anim.SetBool("FallOnFace", false);
        yield return new WaitForSeconds(6f);
        fca.setCamSize(10, 0.006f);
        //c2DF.posFixed = false;
        c2DF.damping = 0.3f;
        yield return new WaitForSeconds(4.5f);
        c2DF.damping = 0.4f;
        //c2DF.posFixed = false;
        //fca.setCamSize(10, 0.01f);
        cc.canMove = true;
    }
}
