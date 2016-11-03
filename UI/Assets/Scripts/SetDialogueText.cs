using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetDialogueText : MonoBehaviour {
	public GameObject titlePanel = null ;
	public GameObject contentPanel = null;
	private Animator animator; 
	private bool shown;

	void Start () {
		animator = GetComponent<Animator> ();
		shown = false; 
	}

	void Update (){
		if (Input.GetKeyDown ("space")) {
			StartCoroutine(ChangeText ("This is a Test", "ABC"));
		}
		if (Input.GetKeyDown ("a")) {
			StartCoroutine(ChangeText ("ABCDEF", "1234512345123451 2345123451234 51234512345123451234512345123451234512345123451234512345"));
		}

		if (Input.GetKeyDown ("b")) {
			RemoveTextBox ();
		}
	}

    public void UpdateText(string text, string title)
    {
        StartCoroutine(ChangeText(text, title));
    }

    IEnumerator ChangeText(string newText, string newTitle) {
		if (shown) {
			// need more to increase the height - transition? 
			animator.SetTrigger ("Fade Out Text");
			yield return new WaitForSeconds(1.3f);
			SetText (newText, newTitle);
			animator.SetTrigger ("Fade In");
			yield return 1; 
		} else {
			SetText (newText, newTitle);
			animator.SetTrigger ("Open");
			shown = true;
		}
	}

	private void SetText(string text, string title) {
		titlePanel.GetComponentInChildren<Text>().text = text;
		contentPanel.GetComponentInChildren<Text>().text = title;
	}

	void RemoveTextBox() {
		shown = false; 
		animator.SetTrigger ("Close");
	}

}
