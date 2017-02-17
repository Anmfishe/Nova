using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthScript : MonoBehaviour {
    public bool grow = false;
    public bool forceFacingRight;
    public bool forceFacingLeft;
    public GameObject prefab;
    private Transform target;
    private bool instantiated = false;
    public SpriteRenderer temporarySprite;
    private GameObject player;
	// Use this for initialization
	void Start () {
        target = transform.Find("TargetTransform");
        player = GameObject.FindGameObjectWithTag("Player");
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
                temporarySprite.enabled = false;
            }
        }
	}
}
