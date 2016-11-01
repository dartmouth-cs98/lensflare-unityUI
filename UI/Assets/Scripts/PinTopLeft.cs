using UnityEngine;
using System.Collections;

public class PinTopLeft : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.94f, .9f, Camera.main.farClipPlane)); 
		gameObject.transform.localEulerAngles = Camera.main.transform.localEulerAngles;
		gameObject.transform.Rotate (new Vector3 (90, 0, 0));
	}

}
