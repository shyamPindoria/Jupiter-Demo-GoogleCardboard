using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the animations that happen in the Airlock Chamber.
/// </summary>
public class Airlock : MonoBehaviour {
	
	/// <summary>
	/// The atom prefab.
	/// </summary>
	public Transform atom;

	/// <summary>
	/// Where to start the positioning of the atoms (parent of atoms).
	/// </summary>
	public Transform startPoint;

	/// <summary>
	/// The point where atoms move towards.
	/// </summary>
	public Transform vacuumPoint;

	/// <summary>
	/// Vaccum door up.
	/// </summary>
	public Transform vacuumDoorUp;

	/// <summary>
	/// Vaccum door down.
	/// </summary>
	public Transform vacuumDoorDown;

	/// <summary>
	/// All the atoms.
	/// </summary>
	private Transform[] molecules;

	/// <summary>
	/// Indicates whether atoms have been generated.
	/// </summary>
	bool atomsGenerated = false;

	/// <summary>
	/// Indicates whether the vacuum door is open.
	/// </summary>
	bool vacuumOpen = false;

	/// <summary>
	/// Glass animator component that moves the glass between chambers.
	/// </summary>
	public Animator glassAnimator;

	/// <summary>
	/// The vacuum door up initial position.
	/// </summary>
	Vector3 vacuumDoorUpInitialPosition;
	/// <summary>
	/// The vacuum door down initial position.
	/// </summary>
	Vector3 vacuumDoorDownInitialPosition;

	// Use this for initialization
	void Start () {
		// Move the glass to the starting position
		glassAnimator.SetTrigger("moveToBase");
		// Store the initial position of the vacuum doors
		vacuumDoorUpInitialPosition = vacuumDoorUp.localPosition;
		vacuumDoorDownInitialPosition = vacuumDoorDown.localPosition;
	}

	/// <summary>
	/// Resets the airlock chamber.
	/// </summary>
	public void resetAirlock () {

		// Play intro audio
		Demonstration.voiceOvers.playAudio (1);

		vacuumOpen = false;
		// Destroy all present atoms
		destroyAtoms ();
		// Move the glass to the start
		glassAnimator.SetTrigger("moveToBase");
		//Reset the position of the vacuum doors
		vacuumDoorUp.localPosition = vacuumDoorUpInitialPosition;
		vacuumDoorDown.localPosition = vacuumDoorDownInitialPosition;

	}

	// Update is called once per frame
	void Update () {
		
		// Disable glass animator if the demonstration is paused
		glassAnimator.enabled = !Demonstration.paused;

		// Check if demonstration has started and that it is not paused
		if (Demonstration.started && !Demonstration.paused) {
			if (!AlertObserver.glassInAirlock && !Demonstration.voiceOvers.isPlaying(1)) {
				// Move glass to airlock if it is not in airlock
				glassAnimator.SetTrigger ("moveToAirlock");

				if (!Demonstration.voiceOvers.isPlaying (2)) {
					// Play airlock audio
					Demonstration.voiceOvers.playAudio (2);
				}

				if (!atomsGenerated) {
					// Generate atoms if they are not yet generated
					generateAtoms ();
				}
			// If glass has moved to airlock chamber
			} else {
				// Check if vacuum is open
				if (vacuumOpen) {
					// Move the atoms to the vacuum
					moveToVacuum ();
					// Check if all atoms have entered the vacuum i.e destroyed
					if (startPoint.childCount == 0) {
						// Close the vacuum door
						closeVacuumDoor ();

						if (!vacuumOpen && !Demonstration.voiceOvers.isPlaying(2)) {
							// Move the glass to deposition once the vacuum is closed
							glassAnimator.SetTrigger ("moveToDeposition1");
							Demonstration.voiceOvers.playAudio (3);
						}
					}
				} else if(!vacuumOpen && atomsGenerated) {
					// Open the vacuum door if atoms are generated and the vacuum is not open
					openVacuumDoor ();
				}
			}
		}

	}

	/// <summary>
	/// Moves the atoms into the vacuum.
	/// </summary>
	void moveToVacuum (){

		// Iterate over all the molecules to move them individually
		foreach (Transform molecule in startPoint) {
			// Position of the vacuum
			Vector3 vacuum = new Vector3 (vacuumPoint.position.x + 0.3f , Random.Range (vacuumPoint.position.y - 0.1f, vacuumPoint.position.y + 0.1f), Random.Range (vacuumPoint.position.z -0.1f, vacuumPoint.position.z + 0.1f));

			// The speed the atoms move into the vacuum
			float step = Random.Range (0.01f, 0.4f) * Time.deltaTime;

			// Distance between the atom and the vacuum point
			float dy = Mathf.Abs(molecule.position.y - vacuumPoint.position.y);
			float dz = Mathf.Abs(molecule.position.z - vacuumPoint.position.z);

			// If the atom is close to the vacuum
			if (dy < 0.1f && dz < 0.1f) {

				// Move it to this point
				vacuum = new Vector3 (-1.1f, 1.4f, 0.1f);

				// If the atom is already in the vacuum point
				if (molecule.position == vacuum) {

					// Destroy the atoms
					Destroy (molecule.gameObject);

					atomsGenerated = false;

				}

			}
			// Move the molecules to the vacuum point
			molecule.position = Vector3.MoveTowards (molecule.position, vacuum, step);
		}
	}

	/// <summary>
	/// Generates the atoms in the chamber.
	/// </summary>
	void generateAtoms () {

		// Generate 1000 atoms
		for (var i = 0; i < 1000; i++) {
			// Random position
			float x = Random.Range (-0.07f, -0.3f);
			float y = Random.Range (0.9f, 1.9f);
			float z = Random.Range (-0.6f, 0.8f);

			// Create a new atom at a random position
			Instantiate(atom, new Vector3 (x, y, z), Quaternion.identity, startPoint);

		}

		atomsGenerated = true;

	}

	/// <summary>
	/// Destroys all present atoms.
	/// </summary>
	public void destroyAtoms () {
		
		foreach (Transform child in startPoint) {
			// Destroy atom
			Destroy (child.gameObject);
		}
		atomsGenerated = false;

	}

	/// <summary>
	/// Opens the vacuum door.
	/// </summary>
	void openVacuumDoor () {
		// Speed of animation
		float step = 0.3f * Time.deltaTime;

		// Move the vacuum doors
		vacuumDoorUp.localPosition = Vector3.MoveTowards (vacuumDoorUp.localPosition, new Vector3 (vacuumDoorUp.localPosition.x, 0.23f, 0.23f) , step);
		vacuumDoorDown.localPosition = Vector3.MoveTowards (vacuumDoorDown.localPosition, new Vector3 (vacuumDoorDown.localPosition.x, -0.23f, -0.23f) , step);

		// Mark the vacuum open once doors are open
		if (vacuumDoorUp.localPosition.y >= 0.23f && vacuumDoorUp.localPosition.z >= 0.23f && vacuumDoorDown.localPosition.y <= -0.23f && vacuumDoorDown.localPosition.z <= -0.23f) {
			vacuumOpen = true;
		}


	}

	/// <summary>
	/// Closes the vacuum door.
	/// </summary>
	void closeVacuumDoor () {
		// Speed of animation
		float step = 0.3f * Time.deltaTime;

		// Move the vacuum doors
		vacuumDoorUp.localPosition = Vector3.MoveTowards (vacuumDoorUp.localPosition, new Vector3 (vacuumDoorUp.localPosition.x, 0.1f, 0.1f) , step);
		vacuumDoorDown.localPosition = Vector3.MoveTowards (vacuumDoorDown.localPosition, new Vector3 (vacuumDoorDown.localPosition.x, -0.1f, -0.1f) , step);

		// Mark the vacuum closed once the doors are closed
		if (vacuumDoorUp.localPosition.y <= 0.1f && vacuumDoorUp.localPosition.z <= 0.1f && vacuumDoorDown.localPosition.y >= -0.1f && vacuumDoorDown.localPosition.z >= -0.1f) {
			vacuumOpen = false;
		}
	}
		
}