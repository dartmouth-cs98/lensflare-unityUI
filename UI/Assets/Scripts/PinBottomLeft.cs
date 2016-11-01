using UnityEngine;
using System.Collections;

public class PinBottomLeft : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.9f, .1f, Camera.main.farClipPlane)); 
	}

}
