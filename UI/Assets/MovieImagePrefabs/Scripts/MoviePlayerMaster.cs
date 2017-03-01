using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePlayerMaster : MonoBehaviour {

	Animator anim;
	MovieController ms;

	string MovieState = "Stop";
	bool IsPlaying = false;

	void Start () {

		anim = GetComponent<Animator> ();
		ms = GetComponentInChildren<MovieController> ();
	}

	public void PlayMovie(string url) {
		if (IsPlaying) {
			ChangeState ();
			ms.StopMovie ();
			IsPlaying = false;
		} else {
			ChangeState ();
			ms.StreamMovie (url);
			IsPlaying = true;
		}


	}

//	void Update () {
//		if (Input.GetKeyDown ("l")) {
//			ms.StreamMovie ("http://www.unity3d.com/webplayers/Movie/sample.ogg");
//		}
//
//		if (Input.GetKeyDown ("p")) {
//			ms.PlayMovie ();
//		}
//
//		if (Input.GetKeyDown ("t")) {
//			ms.StopMovie ();
//		}
//		if (Input.GetKeyDown ("s")) {
//			ms.PauseMovie ();
//		}
//	}

	public void ChangeState() {
		anim.SetTrigger ("StateChange");
	}
}
