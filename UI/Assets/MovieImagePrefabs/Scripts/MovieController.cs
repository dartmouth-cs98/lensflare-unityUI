using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieController : MonoBehaviour {

	MovieTexturePlayer mtp;
	AudioSource aud;

//	bool playing = false;

//	public void LoadMovie(string path) {
//		// remember to comment out stop in movietextureplayer.cs
//		mtp = GetComponent<MovieTexturePlayer> ();
//		mtp.movieTexture = Resources.Load (path) as MovieTexture;
//
//		aud = GetComponent<AudioSource> ();
//		mtp.audioSource = aud;
//	}

	public void StreamMovie(string url) {
		StartCoroutine (movieLoader (url));
	}

	IEnumerator movieLoader(string url) {

		WWW diskMovieDir = new WWW(url);
		MovieTexture mt = diskMovieDir.movie;

		//Wait for file finish loading
		while(!mt.isReadyToPlay){
			yield return null;
		}

		mtp = GetComponent<MovieTexturePlayer> ();
		mtp.movieTexture = mt; 

		aud = GetComponent <AudioSource> ();
		mtp.audioSource = aud;

		mtp.Play ();

	}

	public void PlayMovie () {
//		if (playing) {
//			print ("Already playing movie");
//			return;
//		}
		mtp.Play ();
//		playing = true;
	}

	public void StopMovie() {
		mtp.Stop ();
//		playing = false;
	}

	public void PauseMovie() {
		mtp.Pause ();
//		playing = false;
	}

}
