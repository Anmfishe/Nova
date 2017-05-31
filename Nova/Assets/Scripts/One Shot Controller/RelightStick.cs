using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelightStick : MonoBehaviour {
    public FireNovaController fnc;
    public ParticleSystem ps;
    public FixedCameraAreaScript fca;
    public SpriteRenderer flameAnim;
    private CharacterController cc;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    private Transform target;
    bool first = true;
    bool ready = false;
    int numColliders = 0;
	// Use this for initialization
	void Start () {
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        fnc.anim.Play("FireNovaL2S3SpiderNest");
        c2DF = Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
        target = transform.Find("Target");
	}
	
	// Update is called once per frame
	void Update () {
        //if(!ps.isPlaying)
        //{
        //    
        //}
        if (!ps.isPlaying && first && cc.getDir() && Input.GetKey(KeyCode.E) && ready)
        {
            
            first = false;
            StartCoroutine(lightStick());
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            numColliders++;
            ready = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            numColliders--;
            if(numColliders == 0)
            {
                //Debug.Log("Good");
                ready = false;
            }
            
        }
    }
    IEnumerator lightStick()
    {
        
        cc.canMove = false;
        c2DF.posFixed = true;
        c2DF.target = target;
        fca.setCamSize(8, 0.005f);
        yield return new WaitForSeconds(1);
        cc.anim.Play("NovaL2S3ReigniteTwig");
        yield return new WaitForSeconds(2);
        fnc.anim.Play("FireNovaL2S3SpiderNest2");
        yield return new WaitForSeconds(4);
        flameAnim.color = new Color(1, 1, 1, 1);
        ps.Play();
        ps.GetComponent<AudioSource> ().Play ();
        yield return new WaitForSeconds(2.75f);
        cc.canMove = true;
        c2DF.posFixed = false;
        c2DF.target = cc.gameObject.transform;
        fca.setCamSize(10, 0.01f);
    }
}
