using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float speed = 100f;
	public bool indicator_visible = true;

	Renderer renderer;

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
			transform.Rotate (Vector3.up, speed * Time.deltaTime);
		} else if (indicator_visible && !renderer.enabled) {
			renderer.enabled = true;
		}
		else {
			renderer.enabled = false;
		}
	}
}
