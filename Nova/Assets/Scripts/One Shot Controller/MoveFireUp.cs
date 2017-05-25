using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFireUp : MonoBehaviour {
    [HideInInspector]
    public bool move = false;
    private float rate = 0.025f;
    private FireController fc;
	// Use this for initialization
	void Start () {
        fc = gameObject.GetComponent<FireController>();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		if(move && transform.position.y < -222.7f)
        {
            Vector3 t = transform.position;
            t.y += rate;
            transform.position = t;
        }
       
	}
}
