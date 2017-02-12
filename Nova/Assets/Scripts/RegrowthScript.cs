using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowthScript : MonoBehaviour {
    public bool grow = false;
    public GameObject prefab;
    private Transform target;
    private bool instantiated = false;
    public SpriteRenderer temporarySprite;

	// Use this for initialization
	void Start () {
        target = transform.Find("TargetTransform");
	}
	
	// Update is called once per frame
	void Update () {
		if(grow && !instantiated)
        {
            instantiated = true;
            Instantiate(prefab, target.position, target.rotation, transform);
            temporarySprite.enabled = false;
        }
	}
}
