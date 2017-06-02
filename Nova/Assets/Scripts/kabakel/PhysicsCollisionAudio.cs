using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollisionAudio : MonoBehaviour {

	public AudioClip ImpactSound;
	public float MinImpactVelocity;
	public float MinRepeatTime;
	public AudioSource impactAudioSource;

	private float lastPlayTime;

	void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log ("COLLISION! we hit " + collision.gameObject.name + " with speed " + collision.relativeVelocity.magnitude);

		float currentTime = Time.time;
		bool canRepeat = currentTime >= (lastPlayTime + MinRepeatTime);
		bool isFastEnough = collision.relativeVelocity.magnitude >= MinImpactVelocity;
		if (canRepeat && isFastEnough)
		{
			bool isCharacter = collision.gameObject.GetComponent<CharacterController> () != null;
            bool isGroundCollider = collision.gameObject.tag != "FinalEmberSound";

            if (!isCharacter && isGroundCollider)
			{
				impactAudioSource.PlayOneShot (ImpactSound, 0.7f);
				lastPlayTime = currentTime;
			}
		}
	}
}
