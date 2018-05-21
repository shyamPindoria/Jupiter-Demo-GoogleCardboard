using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class stores the states of some objects and other classes can read this states.
/// </summary>
public class AlertObserver : MonoBehaviour {

	/// <summary>
	/// Gets or sets a value indicating whether glass is in airlock.
	/// </summary>
	/// <value><c>true</c> if glass in airlock; otherwise, <c>false</c>.</value>
	public static bool glassInAirlock {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether glass in deposition1.
	/// </summary>
	/// <value><c>true</c> if glass in deposition1; otherwise, <c>false</c>.</value>
	public static bool glassInDeposition1 {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating whether glass in deposition2.
	/// </summary>
	/// <value><c>true</c> if glass in deposition2; otherwise, <c>false</c>.</value>
	public static bool glassInDeposition2 {
		get;
		set;
	}

	/// <summary>
	/// Gets or sets a value indicating the color of the glass.
	/// </summary>
	/// <value>The color of the glass.</value>
	public static string glassColor {
		get;
		set;
	}

	/// <summary>
	/// Indicates whether teleport mode is active.
	/// </summary>
	/// <value>True if teleport mode is active.</value>
	public static bool teleportMode { 
		get; 
		set; 
	}

	/// <summary>
	/// Indicates whether player is done teleporting.
	/// </summary>
	/// <value>True if teleported.</value>
	public static bool teleported { 
		get; 
		set; 
	}

	// Use this for initialization
	void Start () {
		// Reset all values
		glassInAirlock = false;
		glassInDeposition1 = false;
		glassInDeposition2 = false;
		glassColor = "default";
	}

	/// <summary>
	/// Sets the value for glassInAirlock.
	/// </summary>
	/// <param name="s">String either true or false.</param>
	void setGlassInAirlock(string s) {
		if (s.Equals ("true")) {
			glassInAirlock = true;
		}
		else if (s.Equals ("false")) {
			glassInAirlock = false;
		}
	}

	/// <summary>
	/// Sets the value for glassInDeposition1.
	/// </summary>
	/// <param name="s">String either true or false.</param>
	void setGlassInDeposition1(string s) {
		if (s.Equals ("true")) {
			glassInDeposition1 = true;
		}
		else if (s.Equals ("false")) {
			glassInDeposition1 = false;
		}
	}

	/// <summary>
	/// Sets the value for glassInDeposition2.
	/// </summary>
	/// <param name="s">String either true or false.</param>
	void setGlassInDeposition2(string s) {
		if (s.Equals ("true")) {
			glassInDeposition2 = true;
		}
		else if (s.Equals ("false")) {
			glassInDeposition2 = false;
		}
	}

	/// <summary>
	/// Sets the value for color.
	/// </summary>
	/// <param name="s">String containing the color of the glass.</param>
	void colorChanged(string s) {
		glassColor = s;
	}

	/// <summary>
	/// Sets the value for teleportMode.
	/// </summary>
	/// <param name="value">value to set.</param>
	public void setTeleportMode(bool value) {
		teleportMode = value;
	}

}
