using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores the voice over audio clips and provides methods for playing them.
/// </summary>
public class VoiceOvers : MonoBehaviour {

	// Audio Sources
	public AudioSource intro;
	public AudioSource airlock;
	public AudioSource deposition1;
	public AudioSource deposition2;

	static List<AudioSource> clips;

	// Use this for initialization
	void Start () {

		// Initialise the list and add audio sources to it
		clips = new List<AudioSource> ();
		clips.Add (intro);
		clips.Add (airlock);
		clips.Add (deposition1);
		clips.Add (deposition2);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Play the audio source at the specified index.
	/// </summary>
	/// <param name="index">Index of the audio to play.</param>
	public void playAudio(int index) {
		clips [index-1].Play ();
	}

	/// <summary>
	/// Checks whether an audio source is playing.
	/// </summary>
	/// <returns><c>true</c>, if playing, <c>false</c> otherwise.</returns>
	/// <param name="index">Index of the audio source to check.</param>
	public bool isPlaying(int index) {
		return clips [index-1].isPlaying;
	}

	/// <summary>
	/// Pause all audio sources.
	/// </summary>
	/// <param name="b">If set to <c>true</c> pause.</param>
	public void pause(bool b) {
		foreach (AudioSource audio in clips) {
			if (b) {
				audio.Pause();
			} else {
				audio.UnPause();
			}
		}

	}

	/// <summary>
	/// Stops all audio sources.
	/// </summary>
	public void stopAll() {
		
		foreach (AudioSource audio in clips) {
			audio.Stop ();
		}

	}
}
