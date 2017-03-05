using UnityEngine;

public class ImageControllerMaster : MonoBehaviour {

    ImageController ic;

	// Use this for initialization
	void Start () {
		ic = GetComponentInChildren<ImageController> ();
	}

	public void ShowImage(string url, int width, int height) {
		ic.LoadImage (url, width, height);
	}

}
