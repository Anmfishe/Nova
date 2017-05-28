using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIHandler : MonoBehaviour {

    public bool isPaused;
    public GameObject GUI;
    public GameObject Menu;
    public Button[] buttonarr;
    public AudioClip escapesound;
    public AudioClip selectsound;
    public AudioClip switchsound;
    private AudioSource menuAS;

    // Use this for initialization
    void Start () {
       
        buttonarr[0].enabled = true;
        buttonarr[0].Select();
        buttonarr[0].OnSelect(null);
        
        Debug.Log("Button:-" + buttonarr[0].IsInteractable());

        menuAS = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playClip(escapesound);
            if (GUI.name=="MenuGUI")
            {
                isPaused = !isPaused;
            }
            else
            {
                navigate2(Menu);
                playClip(switchsound);
            }

            Debug.Log("ESP Pressed!!! Pause-"+isPaused);

            buttonarr[0].enabled = true;
            buttonarr[0].Select();
            buttonarr[0].OnSelect(null);

        }
            
        if (isPaused)
        {
            Time.timeScale = 0f;
            GUI.SetActive(true);
        }
            
        else
        {
            Time.timeScale = 1f;
            GUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isPaused == true && GUI.name == "MenuGUI")
            {
                playClip(selectsound);
            }

        }


    }


    //function called for Canvas switch
    public void navigate2(GameObject canv)
    {
     
        if(canv.name == "ResumeGUI")
        {
            playClip(escapesound);
            Debug.Log("NOWNOWNOW " + canv);
            isPaused = false;
        }
        else
        {
            playClip(switchsound);
            GUI.SetActive(false);
            GUI = canv;
            Debug.Log("Game object: " + canv);
         
            for (int i = 0; i < buttonarr.Length; i++)
            {
                Debug.Log("Button:-" + buttonarr[i].IsInteractable() + "i=" + i);
            }
          
        }

    }

    public void playClip(AudioClip soundclip)
    {
        menuAS.clip = soundclip;
        menuAS.Play();
        Debug.Log("Play" + soundclip);
    }

}

