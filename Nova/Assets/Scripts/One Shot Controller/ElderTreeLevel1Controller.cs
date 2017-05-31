using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderTreeLevel1Controller : MonoBehaviour {
    public AudioSource[] AudioToFadeOutAtEnd;
    private GameObject player;
    private Camera cam;
    private ParticleSystem ps;
    bool first = true;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && first)
        {
            first = false;
            StartCoroutine(switchLevels());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            
        }
    }
    IEnumerator switchLevels()
    {
        player.GetComponent<CharacterController>().canMove = false;
        yield return new WaitForSeconds(0.5f);
        player.GetComponent<Animator>().SetBool("EnterLevel", true);
        //play a sound
        yield return new WaitForSeconds(3);
        cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().fadeRate = 0.01f;
        cam.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeOut();

        for (int i=0; i<AudioToFadeOutAtEnd.Length; ++i)
        {
            StartCoroutine(KabakelAudioUtilities.FadeSoundOut(AudioToFadeOutAtEnd[i], 3.8f));
        }

        yield return new WaitForSeconds(4);
        Application.LoadLevel("Level1");
    }
}
