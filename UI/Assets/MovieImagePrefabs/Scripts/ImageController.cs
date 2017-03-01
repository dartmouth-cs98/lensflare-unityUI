using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImageController : MonoBehaviour {

	Animator anim; 

	public void AdjustScaleForImage(int width, int height) {

//		float heightMultiplier = (float)width / height;
//		gameObject.transform.localScale = new Vector3 (heightMultiplier, 1, 1);
		float widthMultiplier = (float)height / width;
		gameObject.transform.localScale = new Vector3 (1, 1, widthMultiplier);
	}

	public void LoadImage(string url, int width, int height) {
		StartCoroutine (ImageLoader(url, width, height));
	}
		
	IEnumerator ImageLoader(string url, int width, int height) {
		anim = GetComponent<Animator> ();

		AdjustScaleForImage (width, height); 

		Texture2D tex;
		tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
		WWW www = new WWW(url);
		yield return www;
		www.LoadImageIntoTexture(tex);
		GetComponent<Renderer>().material.mainTexture = tex;
	}



}
