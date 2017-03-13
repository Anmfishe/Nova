using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthScript : MonoBehaviour {
    [HideInInspector]
    public bool grow = false;
    public bool forceFacingRight;
    public bool forceFacingLeft;
    public GameObject prefab;
    private bool instantiated = false;
    private GameObject player;
    private Transform target;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        target = transform.FindChild("Target");
	}
	
	// Update is called once per frame
	void Update () {
		if(grow && !instantiated)
        {
            if (forceFacingRight && player.GetComponent<CharacterController>().getDir() || forceFacingLeft && !player.GetComponent<CharacterController>().getDir()
                || !forceFacingRight && !forceFacingLeft)
            {
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
}
