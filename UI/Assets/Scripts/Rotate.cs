using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float speed = 100f;
	public bool indicator_visible = true;

	Renderer renderer;

	Vector3 prevRotation;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("t")) {
			indicator_visible = !indicator_visible;
		}

		if (indicator_visible && renderer.enabled) {
//			transform.localEulerAngles.z = Camera.main.transform.localEulerAngles.z;
//			Vector3 temp = transform.localEulerAngles;
//			transform.localEulerAngles = new Vector3 (90, temp.y, temp.z);


			transform.Rotate (Vector3.forward, speed * Time.deltaTime);
			Vector3 temp = transform.localEulerAngles;
			temp.y = Camera.main.transform.localEulerAngles.y;
			temp.x = Camera.main.transform.localEulerAngles.x;
			transform.localEulerAngles = temp;


//			transform.LookAt (Camera.main.transform);

		} else if (indicator_visible && !renderer.enabled) {
			renderer.enabled = true;
		}
		else {
			renderer.enabled = false;
		}
	}
}
