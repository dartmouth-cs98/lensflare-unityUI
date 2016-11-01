using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;


public class ImageTester : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Texture2D text = new Texture2D (400, 287); 
		text.LoadImage(File.ReadAllBytes("./Assets/Images/eiffel-tower.jpg"));
		text.Resize (300,300);
		text.Apply (); 
		GetComponentInChildren<Image> ().sprite = Sprite.Create (text, new Rect (0, 0, 300, 300), new Vector2 (.5f, .5f), 100);





	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
