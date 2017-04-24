using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshController : MonoBehaviour {
    public float min = 2;
    public float max = 7;
    private Rigidbody2D rb2d;
    private bool grounded = false;
    private float switchLayerTimer;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        switchLayerTimer = Random.Range(2, 7);
        //StartCoroutine(switchLayers());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //grounded = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.NameToLayer("Ground"));

    }
    IEnumerator switchLayers()
    {
        yield return new WaitForSeconds(switchLayerTimer);
        if(gameObject.layer == LayerMask.NameToLayer("Ash"))
        {
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
        }
    }
}
