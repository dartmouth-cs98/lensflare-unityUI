using UnityEngine;
using System.Collections;

public class IndicatorControl : MonoBehaviour {
	private bool spinning = false;
	private Animator animator;

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void Update() {
		if (Input.GetKeyDown ("o")) {
			print ("O is pressed - turn on");
			setIndicator (true);
		} else if (Input.GetKeyDown ("p")) {
			print ("P is pressed- turn off");
			setIndicator (false);
		}
	}

	public void setIndicator(bool a) {
		spinning = a;
		animator.SetBool ("Spinning", a);
	}
		
}
