using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageControllerMaster : MonoBehaviour {

	Animator anim;
	ImageController ic;

	bool isShowing = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		ic = GetComponentInChildren<ImageController> ();
//		ChangeState ();
//		ic.LoadImage ("https://docs.unity3d.com/uploads/Main/ShadowIntro.png", 452, 278);
	}

	public void ShowImage(string url, int width, int height) {
		if (isShowing) {
			ChangeState ();
		} else {
			ChangeState ();
			ic.LoadImage (url, width, height);
		}
	}
	
	// Update is called once per frame
//	void Update () {
//
//		if (Input.GetKeyDown ("i")) {
//			ic.LoadImage ("https://docs.unity3d.com/uploads/Main/ShadowIntro.png", 452, 278);
//		}
//
//	}

	public void ChangeState() {
		anim.SetTrigger ("StateChange");
	}
}
