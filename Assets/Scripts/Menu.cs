using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public GameObject menu;
	public Transform mainCamera;

	public void onClick() {

		menu.transform.position = mainCamera.position;
		menu.SetActive (!menu.activeSelf);

	}
}
