using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour {
    public Sprite[] flowers;
    public float fadeRate;
    private SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = flowers[Random.Range(0, flowers.Length - 1)];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        sr.color = new Color(1, 1, 1, sr.color.a - fadeRate);
        if(sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
	}
}
