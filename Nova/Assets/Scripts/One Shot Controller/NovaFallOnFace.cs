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
        
        c2DF.damping = 0.2f;
        
        
        yield return new WaitForSeconds(2.2f);
        yield return new WaitForSeconds(1f);

        
       

    }
}
