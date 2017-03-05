using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPrefabAnimator : MonoBehaviour {

	public Animator gemAnim;
	public Animator imageAnim;
	public Animator videoAnim;
	public Animator textAnim;
	public Animator modelAnim;

	public GameObject modelWrapper;
	public GameObject gem;


	public MoviePlayerMaster mpm;
	public ImageControllerMaster icm;
	public LoadAssets la;
	public SetDialogueText sdt; 

	public IconInfo iconInfo;

	bool gemShowing = true;

	// Use this for initialization
	void Start () {
		trigger (gemAnim);
		//la.StartCoroutine ("Load");
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown("space")) {
			Select();
		}
	}

	void trigger(Animator anim) {
		anim.SetTrigger ("StateChange");
	}

	void setModelScale() {
		Vector3 gemSize = gem.GetComponent<Renderer> ().bounds.size;
		print (gemSize);

		modelWrapper.transform.localScale = gemSize;
		modelWrapper.transform.position = gem.transform.position;

	}

	public void Select() {

		string type = iconInfo.info.media.type;
		string url = iconInfo.info.media.media_url;

		if (gemShowing) {
			setModelScale ();
		}
        print(iconInfo.info.media);
        print(type == "");

		if (type == "video/ogg") {
			trigger (videoAnim);
			mpm.PlayMovie (url);
		} else if (type == "image/jpeg" || type == "image/png")
        {
            trigger (imageAnim);
			icm.ShowImage (url, iconInfo.info.media.width, iconInfo.info.media.height);
		} else if (iconInfo.info.media.type == "") {
			sdt.ChangeText (iconInfo.info.title, iconInfo.info.text);
			trigger (textAnim);

            //	trigger (modelAnim);
		}
        else {
			print ("non valid type");
		}

		trigger (gemAnim);

		gemShowing = !gemShowing;
	}

}
