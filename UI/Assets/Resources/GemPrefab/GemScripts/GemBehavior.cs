using UnityEngine;
using System.Collections;

public class GemBehavior : MonoBehaviour
{

	Animator gem_animator;
	ParticleSystem sparkle;
	Animator canvasAnim;
	SetDialogueText sdt;
	bool canvasShowing;

	void Start ()
	{
		gem_animator = GetComponent<Animator> ();
		sparkle = GetComponentInChildren<ParticleSystem> ();
		GameObject framePanel = GameObject.FindGameObjectWithTag ("FramePanel");
		canvasAnim = framePanel.GetComponentInChildren<Animator> ();
		sdt = framePanel.GetComponentInChildren<SetDialogueText> (); 
		canvasShowing = false; 
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("space")) {
			Select ();
		}
		if (Input.GetKeyDown ("a")) {
			ToggleCanvas ();
		}
		if (Input.GetKeyDown ("b")) {
			SetCanvasText ("ABCDEF", "1234512345123451 2345123451234 51234512345123451234512345123451234512345123451234512345");
		}
	}


	void SetCanvasText (string newText, string newTitle) {
		sdt.ChangeText (newText, newTitle);
	}

	void ToggleCanvas() {
		if (canvasShowing) {
			canvasAnim.SetTrigger ("Close");
			canvasShowing = false;
		} else {
			canvasAnim.SetTrigger ("Open");
			canvasShowing = true;
		}
	}

	void Select()
	{
		gem_animator.SetTrigger ("selectGem");
		sparkle.Play();
	}

}

