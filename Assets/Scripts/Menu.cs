using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	
	public GameObject menu;

	public Transform mainCamera;


	/// <summary>
	/// Called on pointer click.
	/// </summary>
	public void onClick() {
		// Check if the user wants to teleport.
		if (!AlertObserver.teleportMode) {
			
			// To always show the menu infront of the user.
			menu.transform.position = mainCamera.transform.position + (mainCamera.transform.forward * 0.5f);
			menu.transform.LookAt(mainCamera);
			menu.transform.Rotate(0, 180f, 0);

			menu.SetActive (!menu.activeSelf);

		} else {
			// If the user wants to teleport
			menu.SetActive(false);
			// Check if the user has finished teleporting.
			if (AlertObserver.teleported) {
				AlertObserver.teleportMode = false;
				AlertObserver.teleported = false;
			}

		}

	}
}
