using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaAssembleUmbra : MonoBehaviour {
    GameObject player;
    CharacterController cc;
    bool first = true;
    private PickUpController2 shroom;
    private UnitySampleAssets._2D.Camera2DFollow c2Df;
    public FixedCameraAreaScript fca;
    public GameObject stick;
    // Use this for initialization
    void Start () {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        shroom = GetComponent<PickUpController2>();
        c2Df = Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && first && cc.holdingSomething)
        {

            first = false;
            StartCoroutine(assemble());
        }
    }
    IEnumerator assemble()
    {
        cc.canMove = false;
        fca.setCamSize(6, 0.01f);
        c2Df.posFixed = true;
        yield return new WaitForSeconds(1);
        cc.anim.Play("NovaAssembleUmbrella");
        yield return new WaitForSeconds(1);
        transform.SetParent(stick.transform, false);
        shroom.pickUp();       
        yield return new WaitForSeconds(2);
        c2Df.posFixed = false;
        fca.setCamSize(14, 0.01f);
        cc.canMove = true;

    }
}
