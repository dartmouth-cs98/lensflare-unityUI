using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetDialogueText : MonoBehaviour {
	public GameObject titlePanel = null ;
	public GameObject contentPanel = null;
			
	public void ChangeText(string newText, string newTitle) {
		titlePanel.GetComponentInChildren<Text>().text = newText;
		contentPanel.GetComponentInChildren<Text>().text = newTitle;
	}

}
