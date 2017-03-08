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
    string previousType = null;

    Animator openAnim = null;
    bool modelAnimOpen; 

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

	public void trigger(Animator anim) {
		anim.SetTrigger ("StateChange");
	}

	void setModelScale() {
        Vector3 gemSize = gem.GetComponent<Renderer>().bounds.size;
        Vector3 modelSize = modelWrapper.transform.localScale;
        float gemToModelRatio = gemSize.x / modelSize.x;
        modelSize.Set(modelSize.x * gemToModelRatio, modelSize.y * gemToModelRatio, modelSize.z * gemToModelRatio);
        modelWrapper.transform.localScale = modelSize;
        modelWrapper.transform.position = gem.transform.position;
	}

	public void Select() {
		string type = iconInfo.info.media.type;
		string url = iconInfo.info.media.media_url;

        print("Select");
		if (gemShowing) {
			setModelScale ();
		}

        if (openAnim != null)
        {
            trigger(openAnim);
            if (openAnim == videoAnim)
            {
                print("Stopping Movie");
                mpm.StopMovie();
            }

            openAnim = null;
        }
        else if (modelAnimOpen)
        {
            trigger(modelAnim);
            modelAnimOpen = false;
        }
        else if (!iconInfo.info.media.selected)
        {
            type = "text";
            sdt.ChangeText(iconInfo.info.title, iconInfo.info.text);
            openAnim = textAnim;
            trigger(textAnim);
        } else if (type == "video/ogg") {
            print("Playing Movie");
			trigger (videoAnim);
			mpm.PlayMovie (url);
            openAnim = videoAnim;
		} else if (type == "image/jpeg" || type == "image/png" )
        {
            openAnim = imageAnim;
            trigger (imageAnim);
			icm.ShowImage (url, iconInfo.info.media.width, iconInfo.info.media.height);
		} else if (type == "text/plain")
        {
            modelAnimOpen = true; 
            la.DownloadModel(url);
        }
        else {
			print ("non valid type");
		}



        //if (previousType == "video/ogg")
        //{
        //    trigger(videoAnim);
        //}
        //else if (previousType == "image/jpeg" || previousType == "image/png")
        //{
        //    trigger(imageAnim);
        //}
        //else if (previousType == "text/plain")
        //{
        //    trigger(modelAnim);
        //}
        //else if (previousType == "text")
        //{
        //    trigger(textAnim);
        //}


        trigger (gemAnim);
		gemShowing = !gemShowing;
	}

}
