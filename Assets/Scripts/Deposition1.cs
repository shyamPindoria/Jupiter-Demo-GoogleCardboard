using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the animations that happen in the Deposition 1 Chamber.
/// </summary>
public class Deposition1 : MonoBehaviour {

	/// <summary>
	/// Argon atom prefab.
	/// </summary>
	public Transform argonAtom;

	/// <summary>
	/// Start point for the generation of argon atoms this is also where this script is attached.
	/// </summary>
	public Transform startPoint;

	/// <summary>
	/// Start point for the generation of glass atoms.
	/// </summary>
	public Transform glassAtomsStart;

	/// <summary>
	/// Glass atom prefab.
	/// </summary>
	public Transform glassAtom;

	/// <summary>
	/// Glass animator component that moves the glass between chambers.
	/// </summary>
	public Animator glassAnimator;

	/// <summary>
	/// Glass color animator component that chamges the color of the glass.
	/// </summary>
	public Animator glassColorAnimator;

	/// <summary>
	/// Stores the location of each atom in the glass.
	/// </summary>
	Dictionary<Vector3, Transform> glassAtomsMap;

	/// <summary>
	/// Locations in the glass where atoms can be placed.
	/// </summary>
	List<Vector3> glassAtomsMapKeys;

	/// <summary>
	/// Number of argon atoms generated.
	/// </summary>
	int argonAtomsGenerated;

	// Use this for initialization
	void Start () {
		
		// Startup config
		resetDeposition ();

	}

	/// <summary>
	/// Resets the deposition chamber.
	/// </summary>
	public void resetDeposition() {
		
		// Activate this object
		startPoint.gameObject.SetActive (true);
		argonAtomsGenerated = 0;
		// Initialise the collections
		glassAtomsMap = new Dictionary<Vector3, Transform> ();
		glassAtomsMapKeys = new List<Vector3> ();

		// Initialise the dictionary
		initialiseGlassAtoms ();

		// Destroy all present atoms
		foreach (Transform atom in startPoint) {
			Destroy (atom.gameObject);
		}

		// Destroy all material atoms
		GameObject[] materialAtoms = GameObject.FindGameObjectsWithTag ("MaterialAtom1");

		foreach (GameObject atom in materialAtoms) {
			Destroy (atom);
		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
		// Pause the animation of argon atoms
		pauseAnimation (!Demonstration.paused);

		// Check if glass is in deposition 1 and not paused
		if (AlertObserver.glassInDeposition1 && !Demonstration.paused) {
			
			if (argonAtomsGenerated < 180) {
				// Generate 180 argon atoms
				StartCoroutine(generateArgonAtoms ());
				argonAtomsGenerated++;
			}
			// Check whether the argon atom has collided with the donor material
			detectCollisionWithMaterial ();
			// Move glass atoms
			moveGlassAtoms ();

		} else if (!AlertObserver.glassInDeposition1) {
			// Reset deposition if glass is not in deposition 1
			resetDeposition ();
		}

	}

	/// <summary>
	/// Pauses the animation of argon atoms.
	/// </summary>
	/// <param name="state">If game is paused</param>
	void pauseAnimation (bool state) {

		// Get argon atoms
		GameObject[] argonAtoms = GameObject.FindGameObjectsWithTag ("ArgonAtom");

		foreach (GameObject atom in argonAtoms) {
			Animator atomAnimator = atom.GetComponent<Animator>();
			// Disable/Enable the animation depending on the state
			atomAnimator.enabled = state; 
		}
			
	}

	/// <summary>
	/// Creates an argon atom at a random position on the tube.
	/// </summary>
	IEnumerator generateArgonAtoms() {
		yield return new WaitForSeconds (15);
		// Random pos y
		float y = Random.Range (0f, -0.8f);

		// Set the parent
		GameObject parent = new GameObject ();
		parent.transform.SetParent (startPoint);

		// Create the atom
		Transform newAtom = Instantiate(argonAtom, new Vector3 (0, 0, 0), Quaternion.identity, parent.transform);

		// Give the atom a tag so that it can be reterieved later
		newAtom.tag = "ArgonAtom";

		parent.transform.localPosition = new Vector3 (0, y, 0);
	
	}

	/// <summary>
	/// Detects the collision between argon atoms and donor material.
	/// </summary>
	void detectCollisionWithMaterial() {

		// Get all argon atoms
		GameObject[] argonAtoms = GameObject.FindGameObjectsWithTag ("ArgonAtom");

		foreach (GameObject atom in argonAtoms) {

			// Check if the atom is colliding
			if (atom.transform.localPosition.x == 0.085f) {
				
				// Generate 5 material/glass atoms if there is a collision
				for (int i = 0; i < 5; i++) {
					
					Transform newAtom = Instantiate (glassAtom, atom.transform.position, Quaternion.identity, atom.transform.parent);
					// To identify the atom
					newAtom.tag = "MaterialAtom1";
					// Disable the fade animation
					newAtom.GetComponent<Animator> ().enabled = false;

					// For checking whether the atom is added to the map or not
					bool atomAddedToMap = false;
					// Random index to add the map
					int index = Random.Range (0, 900);

					// Loop until the atom is added to the map
					while (!atomAddedToMap) {
						
						if (glassAtomsMap [glassAtomsMapKeys [index]] == null) {
							// Added the atom to the index if the index is null
							glassAtomsMap [glassAtomsMapKeys [index]] = newAtom.transform;
							atomAddedToMap = true;
						} else {
							// Increment the index instead of generating a new random index as it would be inefficient if there is only one space free in the map
							index++;
							// Don't let the index go out of bounds
							if (index >= 900) {
								index = 0;
							}
						}

					}

				}

				// Destroy the argon atom that collided
				Destroy (atom);

			}

		}

	}

	/// <summary>
	/// Moves the glass atoms to their mapped positons from the dictionary.
	/// </summary>
	void moveGlassAtoms() {

		// Speed at which they move
		float step = 0.05f * Time.deltaTime;

		// For checking whether all the atoms are in position
		bool atomsInPosition = true;

		// Iterate over the dictionary
		foreach(KeyValuePair<Vector3, Transform> entry in glassAtomsMap) {

			// Check if a key is mapped
			if (entry.Value != null) {
				// Set parent
				entry.Value.SetParent (glassAtomsStart);
				// Move the glass atoms to the mapped key
				entry.Value.localPosition = Vector3.MoveTowards (entry.Value.localPosition, entry.Key, step);

				if (entry.Value.localPosition != entry.Key) {
					// The atom is not in position
					atomsInPosition = false;
				}

			} else {
				
				// Value is null, meaning that an atom is not in position
				atomsInPosition = false;

			}
		}

		if (atomsInPosition) {
			// Fade all atoms once they are in position
			fadeAtoms ();
		}

	}

	/// <summary>
	/// Fades the glass/material atoms and changes the glass color.
	/// </summary>
	void fadeAtoms() {
		
		foreach (KeyValuePair<Vector3, Transform> entry in glassAtomsMap) {
			// Enable the fade animation of all glass atoms
			entry.Value.GetComponent<Animator> ().enabled = true;

		}

		// Change the glass color
		glassColorAnimator.SetTrigger ("copper");

		if (AlertObserver.glassColor.Equals ("copper") && !Demonstration.voiceOvers.isPlaying(3)) {
			// Move to deposition 2 once the glass has changed color
			glassAnimator.SetTrigger ("moveToDeposition2");
			Demonstration.voiceOvers.playAudio (4);
			startPoint.gameObject.SetActive (false);
		}

	}

	/// <summary>
	/// Initialises the glass atoms dictionary.
	/// </summary>
	void initialiseGlassAtoms() {
		
		float x = 0.015f;
		float y = 0; 
		float z = 0;

		// This will arrange the atoms in a 30x30 array on the glass
		for (var i = 0; i < 30; i++) {
			for (var j = 0; j < 30; j++) {

				// Add a position as a key in the map
				// A glass atom will then move to the mapped position
				Vector3 position = new Vector3 (x, y, z);
				glassAtomsMap.Add (position, null);
				glassAtomsMapKeys.Add (position);
				z -= 0.02f;

			}
			y += 0.02f;
			z = 0;
		}

	}

}