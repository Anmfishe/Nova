using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSceneSwitch : MonoBehaviour {
    public float switchDuration;
    private GameObject player;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    private Transform rightPos;
    private Transform leftPos;
    private Transform rightCamPos;
    private Transform leftCamPos;
    // Use this for initialization
    void Start () {
        rightPos = transform.Find("Right Spawn Point");
        leftPos = transform.Find("Left Spawn Point");
        rightCamPos = transform.Find("Right Cam Spot");
        leftCamPos = transform.Find("Left Cam Spot");
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        c2DF = cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            if(player.transform.position.x < transform.position.x)
            {
                StartCoroutine(SceneSwitch(true));
            }
            else if(player.transform.position.x > transform.position.x)
            {
                StartCoroutine(SceneSwitch(false));
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
    IEnumerator SceneSwitch(bool right)
    {
        c2DF.fadeRate = 0.05f;
        c2DF.startFadeOut();
        player.GetComponent<CharacterController>().canMove = false;
        yield return new WaitForSeconds(switchDuration);
        
        if (right)
        { 
            player.transform.position = rightPos.position;
            cam.transform.position = rightCamPos.position;
        }
        else
        {
            player.transform.position = leftPos.position;
            cam.transform.position = leftCamPos.position;
        }
        
        c2DF.startFadeIn();
        player.GetComponent<CharacterController>().canMove = true;
    }
}
