using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChange : MonoBehaviour {
    public string startSprite;
	// Use this for initialization
	void Start () {
        GetComponent<CanvasAnimation>().ChangeSprite(startSprite, "UploadFlow");
		
	}

}
