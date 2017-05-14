using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFireUp : MonoBehaviour {
    [HideInInspector]
    public bool move = false;
    private float rate = 0.02f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if(move && transform.position.y < -235.4f)
        {
            Vector3 t = transform.position;
            t.y += rate;
            transform.position = t;
        }
	}
}
