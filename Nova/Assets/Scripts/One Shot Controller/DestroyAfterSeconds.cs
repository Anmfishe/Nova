using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {
    public GameObject otherArea;
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            otherArea.SetActive(false);
            StartCoroutine(DestroyAfterSecondsCoroutine());
        }
    }
    IEnumerator DestroyAfterSecondsCoroutine()
    {
        
        yield return new WaitForSeconds(10f);
        otherArea.SetActive(true);
        otherArea.GetComponent<FixedCameraAreaScript>().setCamSave(42);
        
        Destroy(this.gameObject);
    }
}
