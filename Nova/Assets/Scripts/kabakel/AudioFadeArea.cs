using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeArea : MonoBehaviour {

	private GameObject player;
	public AudioFade FadeOut;
	public AudioFade FadeIn;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject == player)
		{
			FadeOut.StartTheFade (startFromCurrentVolume:true);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject == player)
		{
			FadeIn.StartTheFade (startFromCurrentVolume:true);
		}
	}
}
