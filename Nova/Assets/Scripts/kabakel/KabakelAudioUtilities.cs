using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KabakelAudioUtilities {

	// coroutine for fading a sound out over [duration] seconds
	public static IEnumerator FadeSoundOut(AudioSource audioSourceToFade, float duration)
	{
		if (audioSourceToFade != null)
		{
			yield return FadeSound (audioSourceToFade, audioSourceToFade.volume, 0.0f, duration);
		}
	}

	// coroutine for fading a sound in over [duration] seconds
	public static IEnumerator FadeSoundIn(AudioSource audioSourceToFade, float duration)
	{
		if (audioSourceToFade != null)
		{
			yield return FadeSound (audioSourceToFade, 0.0f, audioSourceToFade.volume, duration);
		}
	}

	// coroutine for fading a sound out over [duration] seconds
	public static IEnumerator FadeSound(AudioSource audioSourceToFade, float startVolume, float endVolume, float duration)
	{
		if (audioSourceToFade != null)
		{
			float elapsedTime = 0.0f;
			while (elapsedTime < duration) {
				elapsedTime += Time.deltaTime;
				float volume = Mathf.Lerp (startVolume, endVolume, elapsedTime / duration);
				audioSourceToFade.volume = volume;
				yield return new WaitForEndOfFrame ();
			}
			audioSourceToFade.volume = endVolume;
		}
	}

	// coroutine for fading a sound out over [duration] seconds
	public static IEnumerator FadeSound(AudioSource audioSourceToFade, AnimationCurve fadeCurve)
	{
		if (audioSourceToFade != null)
		{
			float elapsedTime = 0.0f;
			if (fadeCurve != null && fadeCurve.keys.Length > 0)
			{
				Keyframe lastKey = fadeCurve.keys [fadeCurve.keys.Length - 1];
				float duration = lastKey.time;
				float endVolume = lastKey.value;

				while (elapsedTime < duration)
				{
					float volume = fadeCurve.Evaluate (elapsedTime);
					audioSourceToFade.volume = volume;
					elapsedTime += Time.deltaTime;
					yield return new WaitForEndOfFrame ();
				}
				audioSourceToFade.volume = endVolume;
			}
		}
	}

}
