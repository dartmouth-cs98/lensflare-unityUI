using UnityEngine;
using System.Collections;

public class GemBehavior : MonoBehaviour
{

	Animator gem_animator;
	Animator canvasAnim;
    public	SetDialogueText sdt;
	bool canvasShowing;
    GameObject framePanel;
	IconInfo iconInfo;

	MoviePlayerMaster mpm;
	ImageControllerMaster icm;

	void Start ()
	{
		gem_animator = GetComponent<Animator> ();
		iconInfo = GetComponent<IconInfo>();
        
        foreach(Transform child in transform)
        {
            if (child.gameObject.tag == "InfoPanel")
            {
                framePanel = child.gameObject;
                canvasAnim = framePanel.GetComponentInChildren<Animator>();
                //sdt = framePanel.GetComponentInChildren<SetDialogueText>();
            }

			if (child.gameObject.tag == "MoviePlane") {
				mpm = child.GetComponentInChildren<MoviePlayerMaster> ();
			}

			if (child.gameObject.tag == "ImagePlane") {
				icm = child.GetComponentInChildren<ImageControllerMaster> ();
			}
        }
				       
		canvasShowing = false; 
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("space")) {
			Select ();
		}
//		if (Input.GetKeyDown ("a")) {
//			ToggleCanvas ();
//		}
//		if (Input.GetKeyDown ("b")) {
//			SetCanvasText ("ABCDEF", "1234512345123451 2345123451234 51234512345123451234512345123451234512345123451234512345");
//		}
	}


	public void SetCanvasText (string newText, string newTitle) {
		if (sdt == null) {
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
			canvasShowing = true;
		}
	}

	public void Select() {
		string type = iconInfo.info.media.type;

        print("Gem type is " + type);

		string url = iconInfo.info.url;

        print("Gem url is" + url);

		ToggleCanvas ();
		if (type == "video/ogg") {
			mpm.PlayMovie (url);
		} else if (type == "image/jpeg" || type == "image/png") {
			icm.ShowImage (url, iconInfo.info.media.width, iconInfo.info.media.height);
		}
	}
		
}

