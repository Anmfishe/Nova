using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutsceneController : MonoBehaviour {
    [System.Serializable]
    public class CutScene
    {
        public AudioClip music;
        public bool stopMusicAtEnd = true;
        public AnimScene[] animScene;
    }
    [System.Serializable]
    public class AnimScene
    {
        public Sprite scene;
        public float duration = 1.5f;
        public float switchInOutRate = 0.01f;
    }
    public bool skipCutScenes;
    public CutScene[] cutScenes;
    private CutScene currCutScene;
    private SpriteRenderer[] sr;
    private AudioSource audioSource;
    private bool whichRender = false;
    private bool switchingAnims = false;
    private bool fadeOutBoth = false;
    [HideInInspector]
    public bool fadeMusic = false;
    private Sprite nextSprite;
    private float alphaChannel = 0;
    private float switchRate;
    private CharacterController cc;
    private float volFadeRate = 0.0012f;

    // Use this for initialization
    void Start () {
        sr = transform.GetComponentsInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        playCutScene(0);
	}

    // Update is called once per frame
    
	void FixedUpdate () {
		if(switchingAnims)
        {
            
            if(alphaChannel <= 1)
            {
                alphaChannel += switchRate;
                sr[whichRender ? 1 : 0].color = new Color(1f, 1f, 1f, alphaChannel);
                sr[!whichRender ? 1 : 0].color = new Color(1f, 1f, 1f, 1 - alphaChannel);
            }
            else
            {
                alphaChannel = 0;
                whichRender = !whichRender;
                switchingAnims = false;
            }
        }
        else if(fadeOutBoth)
        {
            if (alphaChannel >= 0)
            {
                alphaChannel -= switchRate;
                sr[!whichRender ? 1 : 0].color = new Color(1f, 1f, 1f, alphaChannel);
            }
            else
            {
                //sr[0].sprite = null;
                //sr[1].sprite = null;
                whichRender = false;
                fadeOutBoth = false;
                if (currCutScene.stopMusicAtEnd)
                {
                    fadeMusic = true;
                }
                cc.switchBack();

                //playCutScene(1);
            }
        }
        else if(fadeMusic)
        {
            if (audioSource.isPlaying && audioSource.volume > 0)
            {
                audioSource.volume = audioSource.volume - volFadeRate;
            }
            else
            {
                audioSource.Stop();
                fadeMusic = false;
                //Switch cams
                
            }
        }
	}
    public void playCutScene(int sceneNumber)
    {
        if(skipCutScenes)
        {
            cc.switchBack();
            return;
        }
        sr[0].sprite = null;
        sr[1].sprite = null;
        if(sceneNumber > cutScenes.Length || sceneNumber < 0)
        {
            Debug.Log("No cutscene with that number");
            return;
        }
        else
        {
            currCutScene = cutScenes[sceneNumber];
            playAnimations(cutScenes[sceneNumber]);
        }
    }
    private void playAnimations(CutScene cs)
    {
        if (cs.music != null)
        {
            audioSource.clip = cs.music;
            audioSource.Play();
        }
        
           StartCoroutine(playAnimation(cs.animScene));
        
    }
    private IEnumerator playAnimation(AnimScene[] animScene)
    {
        foreach (AnimScene animationScene in animScene)
        {
            //Debug.Log("Yielding at " + Time.time);
            switchRate = animationScene.switchInOutRate;
            switchAnimation(animationScene.scene);
            yield return new WaitForSeconds(animationScene.duration);
        }
        alphaChannel = 1;
        fadeOutBoth = true;
    }
    private void switchAnimation(Sprite s)
    {
        sr[whichRender ? 1 : 0].sprite = s;
        switchingAnims = true;
    }
}
