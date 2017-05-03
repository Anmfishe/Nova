using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool buildVersion = false;
    public string[] levelsToLoad;
    private AudioSource ads;
    private bool fadeMusic = false;
    // Use this for initialization
    void Start()
    {
        ads = GetComponent<AudioSource>();
        if (buildVersion)
        {
            foreach(string s in levelsToLoad)
            Application.LoadLevelAdditive(s);
        }
    }
}

    // Update is called once per frame
    
