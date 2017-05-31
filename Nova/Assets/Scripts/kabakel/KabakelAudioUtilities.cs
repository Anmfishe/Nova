using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KabakelAudioUtilities {

	// coroutine for fading a sound out over [duration] seconds
	public static IEnumerator FadeSoundOut(AudioSource audioSourceToFade, float duration)
	{
		if (audioSourceToFade != null)
		{
			float elapsedTime = 0.0f;
			float startVolume = audioSourceToFade.volume;
			while (elapsedTime < duration) {
				elapsedTime += Time.deltaTime;
				float volume = Mathf.Lerp (startVolume, 0.0f, elapsedTime / duration);
				audioSourceToFade.volume = volume;
				yield return new WaitForEndOfFrame ();
			}
			audioSourceToFade.volume = 0;
		}
	}

}
