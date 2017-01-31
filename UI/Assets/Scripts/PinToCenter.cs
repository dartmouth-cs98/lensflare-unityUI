using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinToCenter : MonoBehaviour {

	public float x = 0.5f;
	public float y = 0.5f;

	float start;
	public float amp = 0.01f;
	public float speed = 1.75f;


	// Use this for initialization
	void Start () {
		start = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        //		transform.position = new Vector3(transform.position.x, start + amp * Mathf.Sin (speed * Time.time), transform.position.z);
        gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f, Camera.main.farClipPlane));
		//gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y + amp * Mathf.Sin (speed * Time.time), Camera.main.farClipPlane));
	}
}
