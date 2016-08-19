using UnityEngine;
using System.Collections;

public class AudioFadeIn : MonoBehaviour {
	public int approxSecondsToFade = 10;
	private AudioSource audio;

	void Start () {
		audio = GetComponent<AudioSource> ();
	}

	void FixedUpdate ()
	{
		if (audio.volume < 1)
		{
			audio.volume = audio.volume + (Time.deltaTime / (approxSecondsToFade + 1));
		}
		else
		{
			Destroy (this);
		}
	}
}
