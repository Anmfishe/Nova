using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarController : MonoBehaviour {
    public ParticleSystem greenPS;
    public GameObject altar;
    public bool finalAltar = false;
    public string nextLevel;
    public AudioClip altarSound;
    public AudioClip finalAltarSound;
    public AudioSource audioSource;
    public AudioSource audioSourceFinal;
    public AudioSource[] audioToFadeOutAtEnd;
    private GameObject player;
    private GameObject ballOfLight;
    private UnityEngine.PostProcessing.PostProcessingBehaviour ppb;
    private SpriteRenderer[] novaSrs;
    bool first = true;
    bool lowerColor = false;
    float novaColorSave;
    float lowerColorRate = 0.01f;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        ppb = Camera.main.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        ballOfLight = GameObject.FindGameObjectWithTag("BallOfLight");
        novaSrs = player.GetComponentsInChildren<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(lowerColor && novaSrs[0].color.r > novaColorSave - .15f)
        {
            foreach(SpriteRenderer sr in novaSrs)
            {
                sr.color = new Color(sr.color.r - lowerColorRate, sr.color.g - lowerColorRate, sr.color.b - lowerColorRate, 1);
            }
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
        player.GetComponent<CharacterController>().canMove = false;
        if (audioSource != null && altarSound != null)
        {
            audioSource.clip = altarSound;
            audioSource.Play ();
        }
        if (finalAltar)
            player.GetComponent<CharacterController>().anim.speed = 0.75f;
        yield return new WaitForSeconds(1);
        player.GetComponent<Animator>().Play("NovaAltarInteraction");
        yield return new WaitForSeconds(1.5f);
        if(finalAltar)
            ballOfLight.GetComponent<Animator>().speed = 0.75f;
        else
        {
            novaColorSave = novaSrs[0].color.r;
            lowerColor = true;
        }
        ballOfLight.GetComponent<Animator>().Play("GlowingBallAnim");
        yield return new WaitForSeconds(2);
        if (!finalAltar)
        {
            altar.GetComponent<Animator>().Play("AltarAnim");
        }
        else
        {
            altar.GetComponent<Animator>().Play("Altar2");
        }
        if (!finalAltar)
        {
            yield return new WaitForSeconds(1.5f);
            greenPS.Play();
            yield return new WaitForSeconds(6.5f);

            Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().fadeRate = 0.0025f;
            Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().startFadeOut();
            float waitDuration = 6;
            for (int i = 0; i < audioToFadeOutAtEnd.Length; ++i)
            {
                StartCoroutine (KabakelAudioUtilities.FadeSoundOut (audioToFadeOutAtEnd[i], waitDuration - 0.2f));
            }
            yield return new WaitForSeconds(waitDuration);
            Application.LoadLevel(nextLevel);
        }
        else
        {
            //audioSource.PlayOneShot (finalAltarSound, 0.7f);
            yield return new WaitForSeconds(2.4f);
            if (audioSourceFinal != null && finalAltarSound != null)
            {
                audioSourceFinal.clip = finalAltarSound;
                audioSourceFinal.Play ();
            }
            Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().fadeRate = 0.05f;
            Camera.main.GetComponent<UnitySampleAssets._2D.Camera2DFollow>().whiteFadeOut();
            float waitDuration = 5;
            for (int i = 0; i < audioToFadeOutAtEnd.Length; ++i)
            {
                StartCoroutine (KabakelAudioUtilities.FadeSoundOut (audioToFadeOutAtEnd[i], waitDuration - 0.2f));
            }
            yield return new WaitForSeconds(waitDuration);
            Application.LoadLevel(nextLevel);
        }
    }
}
