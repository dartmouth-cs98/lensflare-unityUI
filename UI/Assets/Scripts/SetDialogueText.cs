using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetDialogueText : MonoBehaviour {
	private Animator animator; 
	private bool shown;


	// Use this for initialization
	void Awake () {
		animator = GetComponent<Animator> ();
		ChangeText ("This is a Test"); 
		shown = false; 

		// to display animation on run
		animator.SetTrigger ("Fade Out Text");
		animator.SetTrigger ("Fade In Text");
		animator.SetTrigger ("Close");
	}

	void ChangeText(string newText) {
		if (shown) {
			animator.SetTrigger ("Fade Out Text");
			//change the text
			animator.SetTrigger ("Fade In Text");
		} else {
			// set the text 
			animator.SetTrigger ("Open");
			shown = true;
		}
	}

	void RemoveTextBox() {
		shown = false; 
		animator.SetTrigger ("Closed");
	}

}
