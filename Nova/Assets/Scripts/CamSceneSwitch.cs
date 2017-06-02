using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSceneSwitch : MonoBehaviour {
    public float switchDuration;
    public bool resetCamHeight = true;
    public int newCamSize = 10;
    private GameObject player;
    private Camera cam;
    private UnitySampleAssets._2D.Camera2DFollow c2DF;
    private Transform rightPos;
    private Transform leftPos;
    private Transform rightCamPos;
    private Transform leftCamPos;
    private int numColliders = 0;
    private bool first = true;
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
            numColliders++;
            if (numColliders == 1)
            {
                first = false;
                if (player.transform.position.x < transform.position.x)
                {
                    StartCoroutine(SceneSwitch(true));
                }
                else if (player.transform.position.x > transform.position.x)
                {
                    StartCoroutine(SceneSwitch(false));
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            numColliders--;
        }
    }
    IEnumerator SceneSwitch(bool right)
    {
        c2DF.fadeRate = 0.05f;
        c2DF.startFadeOut();
        float dampSave = c2DF.damping;
        c2DF.damping = 0;
        //player.GetComponent<CharacterController>().canMove = false;
        player.GetComponent<CharacterController>().hardStopNova(true);
        yield return new WaitForSeconds(switchDuration/2);
        
        if (right)
        {
            cam.orthographicSize = newCamSize;
            player.transform.position = rightPos.position;
            cam.transform.position = rightCamPos.position;
            if(resetCamHeight)
                c2DF.shiftCamToNova();

        }
        else
        {
            cam.orthographicSize =newCamSize;
            player.transform.position = leftPos.position;
            cam.transform.position = leftCamPos.position;
            if (resetCamHeight)
                c2DF.shiftCamToNova();
            //c2DF.moveCameraHeight(player.transform.position.y - c2DF.aboveNovaConst);
        }
        //if(resetCamHeight)
        //{
        //    c2DF.shiftCamToNova();
        //}
        c2DF.damping = dampSave;
        yield return new WaitForSeconds(switchDuration / 2);
        c2DF.startFadeIn();
        //player.GetComponent<CharacterController>().canMove = true;
        player.GetComponent<CharacterController>().hardStopNova(false);
    }
}
