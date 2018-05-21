using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public GameObject menu;

	public void onClick() {
		menu.SetActive (!menu.activeSelf);

		print ("Clicked.");
	}
}
