using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickExtinguished : MonoBehaviour {
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && first)
        {
            first = false;
            StartCoroutine(ExtinguishStick());
        }
    }
    IEnumerator ExtinguishStick()
    {
        cc.canMove = false;
        yield return new WaitForSeconds(1f);
        float dampSave = c2DF.damping;
        c2DF.damping = 0.8f;
        c2DF.posFixed = true;
        fca.setCamSize(8, 0.005f);
        cc.anim.Play("StickGoesOut");
        yield return new WaitForSeconds(3.5f);
        c2DF.damping = 0.3f;
        c2DF.posFixed = false;
        fca.setCamSize(10, 0.005f);
        cc.canMove = true;
    }
}
