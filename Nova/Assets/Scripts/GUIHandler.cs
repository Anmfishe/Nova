using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIHandler : MonoBehaviour {

    public bool isPaused;   //To check whether the game is paused
    public GameObject GUI;  //Current selected GUI
    public GameObject Menu; //Main menu in GUI
    public Button[] buttonarr;  //Buttons associated with the menu
    public AudioClip escapesound;   
    public AudioClip selectsound;
    public AudioClip switchsound;
    private AudioSource menuAS;

    // Use this for initialization
    void Start () {
       
        //Set "Resume" as default option for beginning
        buttonarr[0].enabled = true;
        buttonarr[0].Select();
        buttonarr[0].OnSelect(null);

        menuAS = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playClip(escapesound);
            if (GUI.name=="MenuGUI")    //Handle game pause only if current GUI is menu
            {
                isPaused = !isPaused;
            }
            else
            {
                navigate(Menu);    //Otherwise switch back to main menu
                playClip(switchsound);
            }

            //Set "Resume" as default option for every switch
            buttonarr[0].enabled = true;
            buttonarr[0].Select();
            buttonarr[0].OnSelect(null);

        }
            
        if (isPaused)
        {
            Time.timeScale = 0f;    //Pause game
            GUI.SetActive(true);
        }
            
        else
        {
            Time.timeScale = 1f;    //Unpause game
            GUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isPaused == true && GUI.name == "MenuGUI")  //Activate GUI keyboard sound only if game is paused and current GUI is menu
            {
                playClip(selectsound);
            }

        }
    }


    //function called for Canvas switch
    public void navigate(GameObject canv)
    {
     
        if(canv.name == "ResumeGUI")
        {
            playClip(escapesound);
            isPaused = false;
        }
        else
        {
            playClip(switchsound);
            GUI.SetActive(false);
            GUI = canv;          
        }

    }

    //Function called for Keyboard/ Mouse control sound
    public void playClip(AudioClip soundclip)
    {
        menuAS.clip = soundclip;
        menuAS.Play();
    }

}

