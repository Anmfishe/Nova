using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbersScript : MonoBehaviour {
    public bool turnToBlack;
    public bool turnAlpha;
    public bool randomizeRotation;
    private SpriteRenderer sr;
    private float rate;
    private float one = 1;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        rate = Random.Range(0.00075f, 0.015f);
        rate *= -1;
        if(randomizeRotation)
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Random.Range(0f, 360f));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        one += rate;
        if(one <= 0 || one >= 1)
        {
            rate *= -1;
        }
        float currVal = Mathf.Lerp(1, 0.25f, one);
        if (turnToBlack)
        {
            sr.color = new Color(currVal, currVal, currVal, 1f);
        }
        if (turnAlpha || !turnAlpha && !turnToBlack)
        {
            sr.color = new Color(1f, 1f, 1f, currVal);
        }
	}
}
