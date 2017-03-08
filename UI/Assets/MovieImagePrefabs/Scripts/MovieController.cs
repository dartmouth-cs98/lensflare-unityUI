using System.Collections;
using UnityEngine;

public class MovieController : MonoBehaviour {

	MovieTexturePlayer mtp;
	AudioSource aud;
    WWW diskMovieDir = null;
    string previousUrl = null; 

	public void StreamMovie(string url) {
		StartCoroutine (movieLoader (url));
	}

	IEnumerator movieLoader(string url) {

        if (diskMovieDir == null || url != previousUrl)
        {
            diskMovieDir = new WWW(url);
            MovieTexture mt = diskMovieDir.movie;

            //Wait for file finish loading
            while (!mt.isReadyToPlay)
            {
                yield return null;
            }

            mtp = GetComponent<MovieTexturePlayer>();
            mtp.movieTexture = mt;

            aud = GetComponent<AudioSource>();
            mtp.audioSource = aud;
        }
        previousUrl = url;
        StopMovie();
        PlayMovie();
	}

	public void PlayMovie () {
		mtp.Play ();
	}

	public void StopMovie() {
		mtp.Stop ();
	}

	public void PauseMovie() {
		mtp.Pause ();
	}

}
