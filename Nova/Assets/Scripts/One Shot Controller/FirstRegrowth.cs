using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRegrowth : MonoBehaviour {
    public Sprite e;
    public Sprite lShift;
    public Sprite anyKey;
    public bool opening = false;
    private GameObject player;
    private CharacterController cc;
    private SpriteRenderer sr;
    private float currentAlpha = 0;
    private int tutorialTimer = 0;
    private float waitTime = 0;
    private bool first = true;
    private AudioSource uiAS;
    bool _showE = false;
    bool _showLShift = false;

    // Use this for initialization
    void Start () {
       

        player = GameObject.FindGameObjectWithTag("Player");
        cc = player.GetComponent<CharacterController>();
        sr = GetComponent<SpriteRenderer>();
        sr.material.color = new Color(1f, 1f, 1f, currentAlpha);
        if (opening)
            sr.sprite = anyKey;
        uiAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cc.canBurn && !cc.hasBurned)
        {
            sr.sprite = e;
        }
        if(cc.canPush && !cc.hasPushed)
        {
            sr.sprite = lShift;
        }
        if(opening && Input.anyKey)
        {
            opening = false;
        }
        if (cc.canBurn && !cc.hasBurned || cc.canPush && !cc.hasPushed || opening && !Input.anyKey)
        {
            if (tutorialTimer < waitTime)
                tutorialTimer++;
            if (tutorialTimer >= waitTime)
            {
                if (currentAlpha < 1)
                {
                    currentAlpha += 0.01f;
                    if (currentAlpha > 0.75 && first)
                    {

                        //uiAS.Play();
                        first = false;

                    }
                }
                
            }
            
        }
        else
        {
            if (currentAlpha > 0)
            {
                currentAlpha -= 0.01f;
            }
        }
        sr.material.color = new Color(1f, 1f, 1f, currentAlpha);
    }
    public void showE(bool b)
    {

    }
    public void showLShift(bool b)
    {

    }
}
