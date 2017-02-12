using UnityEngine;
using System.Collections;

public class GemBehavior : MonoBehaviour
{

	Animator gem_animator;
	ParticleSystem sparkle;
	Animator canvasAnim;
    public	SetDialogueText sdt;
	bool canvasShowing;
    GameObject framePanel;

	void Start ()
	{
		gem_animator = GetComponent<Animator> ();
		sparkle = GetComponentInChildren<ParticleSystem> ();
		//GameObject framePanel = GameObject.FindGameObjectWithTag ("FramePanel");
        
        foreach(Transform child in transform)
        {
            if (child.gameObject.tag == "InfoPanel")
            {
                framePanel = child.gameObject;
                canvasAnim = framePanel.GetComponentInChildren<Animator>();
                //sdt = framePanel.GetComponentInChildren<SetDialogueText>();
              
            }
        }


		//canvasAnim = framePanel.GetComponent<Animator> ();

        print("FP:" + framePanel);
        print("CA:" + canvasAnim);
		//sdt = framePanel.GetComponent<SetDialogueText> ();

        print("SDT  After instantiate:" + sdt);
       
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


	public void SetCanvasText (string newText, string newTitle) {
        if (sdt == null)
        {
            print("sdt is null");

        }
		sdt.ChangeText (newText, newTitle);
	}

	public void ToggleCanvas() {
        print("toggle Canvas" + canvasShowing.ToString());
		if (canvasShowing) {
			canvasAnim.SetTrigger ("Close");
			canvasShowing = false;
		} else {
            print("opening");
            canvasAnim.SetTrigger ("Open");
            //canvasAnim.SetBool("OpenCanvas", true);
			canvasShowing = true;
		}
	}

	public void Select()
	{
		gem_animator.SetTrigger ("selectGem");
		sparkle.Play();
	}

}

