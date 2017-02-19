using UnityEngine;
using System.Collections;

public class PinTopLeft : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.75f, .98f, Camera.main.nearClipPlane + 2)); 
		//gameObject.transform.localEulerAngles = Camera.main.transform.localEulerAngles;
	}

}
