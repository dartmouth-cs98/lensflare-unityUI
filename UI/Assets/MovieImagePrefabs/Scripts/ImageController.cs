using System.Collections;
using UnityEngine;

public class ImageController : MonoBehaviour {

	Animator anim;
    string previousUrl = null;
    WWW request = null;

	public void AdjustScaleForImage(int width, int height)
    {
		float widthMultiplier = (float)height / width;
		gameObject.transform.localScale = new Vector3 (1, 1, widthMultiplier);
	}

	public void LoadImage(string url, int width, int height) {
		StartCoroutine (ImageLoader(url, width, height));
	}
		
	IEnumerator ImageLoader(string url, int width, int height) {
        if (request == null || url != previousUrl)
        {
            anim = GetComponent<Animator>();
            AdjustScaleForImage(width, height);
            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
            request = new WWW(url);
            yield return request;
            request.LoadImageIntoTexture(tex);
            GetComponent<Renderer>().material.mainTexture = tex;
        }
        previousUrl = url;

	}



}
