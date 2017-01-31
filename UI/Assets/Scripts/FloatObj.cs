using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObj : MonoBehaviour {

	float start;
	public float amp = 0.1f;
	public float speed = 2.75f;

	// Use this for initialization
	void Start () {
		start = transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, start + amp * Mathf.Sin (speed * Time.time), transform.position.z);
	}
}
