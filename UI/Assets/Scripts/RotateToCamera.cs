using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

//		transform.Rotate (Vector3.forward, speed * Time.deltaTime);
		Vector3 temp = transform.localEulerAngles;
		temp.y = Camera.main.transform.localEulerAngles.y;
		temp.x = Camera.main.transform.localEulerAngles.x;
		transform.localEulerAngles = temp;
		
	}
}
