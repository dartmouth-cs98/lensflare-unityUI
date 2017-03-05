using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextCanvasBackgroundPanel : MonoBehaviour {

	public RectTransform canvasTrans;

	Vector3 startScale;
	float startRectHeight;

	// Use this for initialization
	void Start () {
		startRectHeight = canvasTrans.rect.height;
		startScale = gameObject.transform.localScale;
		startScale.z = startRectHeight/10.157f;
		gameObject.transform.localScale = startScale;
	}

	// Update is called once per frame
	void Update () {
		if (startRectHeight != canvasTrans.rect.height)
		{
			startRectHeight = canvasTrans.rect.height;
			startScale.z = startRectHeight/10.157f;
		}

		Vector3 cts = canvasTrans.transform.localScale;

		gameObject.transform.localScale = new Vector3 (startScale.x * cts.x, startScale.y * cts.y, startScale.z * cts.z);

	}
}