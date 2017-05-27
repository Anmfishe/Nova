using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthScript : MonoBehaviour {
    [HideInInspector]
    public bool grow = false;
    public bool forceFacingRight;
    public bool forceFacingLeft;
    public GameObject prefab;
    public AudioClip[] regrowthSounds;
    private bool instantiated = false;
    private GameObject player;
    private Transform target;
    private AudioSource regrowAS;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        target = transform.Find("Target");
        regrowAS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(grow && !instantiated)
        {
            if (forceFacingRight && player.GetComponent<CharacterController>().getDir() || forceFacingLeft && !player.GetComponent<CharacterController>().getDir()
                || !forceFacingRight && !forceFacingLeft)
            {
                playRegrowthSound();
                instantiated = true;
                Instantiate(prefab, target.position, target.rotation, transform);
            }
        }
	}
    public bool didGrow
    {
        get { return instantiated; }
        set { instantiated = value; }
    }
    private void playRegrowthSound()
    {
        regrowAS.clip = regrowthSounds[Random.Range(0, regrowthSounds.Length)];
        regrowAS.Play();
    }
    IEnumerator playSoundRoutine()
    {
        yield return new WaitForSeconds(0.75f);
        playRegrowthSound();
    }
}
