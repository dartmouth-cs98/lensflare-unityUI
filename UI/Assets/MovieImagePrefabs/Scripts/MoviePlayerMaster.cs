using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePlayerMaster : MonoBehaviour {

	MovieController ms;
	bool IsPlaying = false;

	void Start () {
		ms = GetComponentInChildren<MovieController> ();
	}

	public void PlayMovie(string url) {
		if (IsPlaying) {
			ms.StopMovie ();
			IsPlaying = false;
		} else {
			ms.StreamMovie (url);
			IsPlaying = true;
		}
	}
}
