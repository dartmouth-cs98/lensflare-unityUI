using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimation : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Awake () {
        print("Loading canvas...1");
        anim = GetComponent<Animator> ();
        print("Loading canvas...2");
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			ShrinkCanvas ();
		}

		if (Input.GetKeyDown ("s")) {
			GrowCanvas ();
		}
		if (Input.GetKeyDown ("a")) {
			ChangeSprite ("get_started", "UploadFlow");
		}
	}

	public void ShrinkCanvas() {
		anim.SetTrigger ("StateChange");
	}
    
	public void GrowCanvas() {
        print("Grow anim" + anim.ToString());
		anim.SetTrigger("StateChange");
	}

	public void ChangeSprite(string imgName, string tag) {
		GameObject sprite = GameObject.FindGameObjectWithTag (tag);
		SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer> ();
		renderer.sprite = Resources.Load <Sprite> (imgName);
	}


	void ShowCanvas() {

	}
		
}
