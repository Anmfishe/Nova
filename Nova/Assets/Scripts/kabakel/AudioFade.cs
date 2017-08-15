using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{

	public List<AudioSource> SoundsToFade;
	private List<float> BaseSoundVolumes;
	private List<Coroutine> Coroutines;

	public bool TriggerOnStart = true;
	public float FadeDuration = 1.0f;
	public float FadeStartPercent = 0.0f;
	public float FadeEndPercent = 1.0f;

	public bool UseCurveForFade = false;
	public AnimationCurve FadeCurve = AnimationCurve.Linear(0,0,0,1);


	void Awake()
	{
		BaseSoundVolumes = new List<float> ();
		Coroutines = new List<Coroutine> ();

		foreach (AudioSource audioSource in SoundsToFade)
		{
			BaseSoundVolumes.Add (audioSource.volume);
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (TriggerOnStart)
		{
			StartTheFade (false);
		}
	}

	public void StartTheFade(bool startFromCurrentVolume)
	{
		StopFades ();

		for (int i = 0; i < SoundsToFade.Count; ++i)
		{
			if (UseCurveForFade)
			{
				if (startFromCurrentVolume) {
					Debug.LogWarning ("StartTheFade with a FadeCurve does not support the startFromCurrentVolume option");
				}
				Coroutines.Add (StartCoroutine (KabakelAudioUtilities.FadeSound (SoundsToFade [i], FadeCurve)));
			}
			else
			{
				float startVolume = startFromCurrentVolume ? SoundsToFade[i].volume : FadeStartPercent * BaseSoundVolumes [i];
				float endVolume = FadeEndPercent * BaseSoundVolumes [i];
				//Debug.Log("Start a fade on " + SoundsToFade[i].name + " from " + startVolume + " to " + endVolume + " over " + FadeDuration + " seconds");
				Coroutines.Add(StartCoroutine (KabakelAudioUtilities.FadeSound (SoundsToFade [i], startVolume, endVolume, FadeDuration)));
			}
		}
	}

	void StopFades()
	{
		for (int i = 0; i < Coroutines.Count; ++i) {
			Coroutine c = Coroutines [i];
			if (c != null) {
				StopCoroutine (c);
			}
		}

		Coroutines.Clear ();
	}
}
