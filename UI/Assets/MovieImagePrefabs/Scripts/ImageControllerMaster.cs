using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageControllerMaster : MonoBehaviour {

	ImageController ic;

	bool isShowing = false;

	// Use this for initialization
	void Start () {
		ic = GetComponentInChildren<ImageController> ();
	}

	public void ShowImage(string url, int width, int height) {
		ic.LoadImage (url, width, height);
	}

}
