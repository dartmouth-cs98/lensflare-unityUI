using UnityEngine;
using System.Collections;

public class GemBehavior : MonoBehaviour
{

	Animator gem_animator;
	ParticleSystem sparkle;

	void Start ()
	{

		gem_animator = GetComponent<Animator> ();
		sparkle = GetComponentInChildren<ParticleSystem> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("space")) {
			Select ();
			
		}
	}

	public void Select()
	{
		gem_animator.SetTrigger ("selectGem");
		sparkle.Play();
	}

}

