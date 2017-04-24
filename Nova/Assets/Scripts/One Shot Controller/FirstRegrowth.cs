using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRegrowth : MonoBehaviour {
    public Sprite e;
    public bool usesBranch;
    public GameObject brokenBranch;
    private HingeJoint2D hj2d = null;
    private GameObject player;
    private SpriteRenderer sr;
    private float currentAlpha = 0;
    private int tutorialTimer = 0;
    private float waitTime = 0;
    private bool first = true;
    private AudioSource uiAS;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        sr.material.color = new Color(1f, 1f, 1f, currentAlpha);
        sr.sprite = e;
        if(usesBranch)
        hj2d = brokenBranch.GetComponent<HingeJoint2D>();
        uiAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.GetComponent<CharacterController>().canGrow() && !player.GetComponent<CharacterController>().getHasGrown()
            && hj2d == null)
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

                        uiAS.Play();
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
}
