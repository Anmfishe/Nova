using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusicOnEnter : MonoBehaviour {
    private AudioSource audioSource;
    private GameObject player;
    private UnityEngine.PostProcessing.PostProcessingBehaviour ppb;
    bool first = true;
    bool lowerBlume = false;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        ppb = Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(lowerBlume)
        {
            //ppb.profile.bloom. = UnityEngine.PostProcessing.BloomModel.BloomSetti;
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player && first)
        {
            first = false;
            StartCoroutine(BackToElder());
        }
    }
    IEnumerator BackToElder()
    {
        audioSource.Play();
        yield return new WaitForSeconds(1);
        player.GetComponent<Animator>().Play("NovaAltarInteraction");
        yield return new WaitForSeconds(10);
        Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeOut();
        yield return new WaitForSeconds(6);
        Application.LoadLevel("ElderTree2");
    }
}
