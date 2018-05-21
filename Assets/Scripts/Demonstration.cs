using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is attached to the menu and is responsible for starting, pausing the demonstration and toggling the cameras.
/// </summary>
public class Demonstration : MonoBehaviour {

	Airlock airlock;
	Deposition1 deposition1;
	Deposition2 deposition2;

	public static VoiceOvers voiceOvers;

	// Contains the three camera monitors
	GameObject[] monitors;

	// Glass color animator
	public Animator glassColorAnimator;

	/// <summary>
	/// Gets or sets a value indicating whether demonstration is paused.
	/// </summary>
	/// <value><c>true</c> if paused; otherwise, <c>false</c>.</value>
	public static bool paused {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether demonstration has started.
	/// </summary>
	/// <value><c>true</c> if started; otherwise, <c>false</c>.</value>
	public static bool started {
		get;
		set;
	}

	void Start() {

		// Get the class object instances or the three chambers
		var airlockObject = GameObject.FindGameObjectWithTag ("Airlock");
		airlock = airlockObject.GetComponent<Airlock> ();

		var deposition1Object = GameObject.FindGameObjectWithTag ("Deposition1");
		deposition1 = deposition1Object.GetComponent<Deposition1> ();

		var deposition2Object = GameObject.FindGameObjectWithTag ("Deposition2");
		deposition2 = deposition2Object.GetComponent<Deposition2> ();

		var voiceOversObject = GameObject.FindGameObjectWithTag ("VoiceOvers");
		voiceOvers = voiceOversObject.GetComponent<VoiceOvers> ();

		// Get the monitors
		monitors = GameObject.FindGameObjectsWithTag ("CloseUpScreen");

	}

	/// <summary>
	/// Starts the demonstration process.
	/// Resets the chambers and some animations.
	/// </summary>
	public void startDemonstration() {
		
		paused = false;

		// Stop voice overs
		voiceOvers.stopAll ();

		// Reset chambers
		airlock.resetAirlock ();
		deposition1.resetDeposition ();
		deposition2.resetDeposition ();

		// Reset glass color
		glassColorAnimator.ResetTrigger ("copper");
		glassColorAnimator.ResetTrigger ("silver");
		glassColorAnimator.SetTrigger ("default");

		// Mark the Demonstration as started
		started = true;

	}

	/// <summary>
	/// Pauses the demonstration.
	/// </summary>
	public void pauseDemonstration() {
		
		paused = !paused;
		voiceOvers.pause (paused);
		glassColorAnimator.enabled = !paused;
	
	}

	/// <summary>
	/// Activates teleport mode.
	/// </summary>
	public void activateTeleport() {
		AlertObserver.teleportMode = true;
	}

	/// <summary>
	/// Toggles the cameras.
	/// </summary>
	public void toggleCameras() {

		// Disable all the monitors
		foreach (GameObject monitor in monitors) {
			monitor.SetActive (!monitor.activeSelf);
		}

	}

}