using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePlayerMaster : MonoBehaviour {

	MovieController ms;

	void Start () {
		ms = GetComponentInChildren<MovieController> ();
	}

	public void PlayMovie(string url) {
			ms.StreamMovie (url);
	}

    public void StopMovie()
    {
        ms.StopMovie();
    }
}
