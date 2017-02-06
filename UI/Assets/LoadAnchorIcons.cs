using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using UnityEngine.Windows.Speech;
using System.Linq;
public class LoadAnchorIcons : MonoBehaviour {

    WorldAnchorStore store;
    SurfaceObserver surfaceObserver;
    LoadIconData lid;
    bool instantiated = false;
    Dictionary<string, GameObject> icons;

	// Use this for initialization
	void Start () {
        surfaceObserver = new SurfaceObserver();
        surfaceObserver.SetVolumeAsAxisAlignedBox(Vector3.zero, new Vector3(3, 3, 3));

        lid = GetComponent<LoadIconData>();
        icons = new Dictionary<string, GameObject>();
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);

        StartCoroutine(UpdateLoop());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSurfaceChanged(SurfaceId surfaceId, SurfaceChange changeType, Bounds bounds, System.DateTime updateTime)
    {
        print("surface changed");
    }

    IEnumerator UpdateLoop()
    {
        print("in update loop");
        var wait = new WaitForSeconds(2.5f);
        while (true)
        {
            surfaceObserver.Update(OnSurfaceChanged);
            yield return wait;
        }
    }

    public WorldAnchorStore GetAnchorStore()
    {
        return this.store;
    }

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;

        string[] anchorIds = this.store.GetAllIds();

        if (lid.iconDonwload == null)
        {
            StartCoroutine(UpdateLoop());
            return;
        }
        print("loadanchoricons " + anchorIds.Length + " " + instantiated);

        for (int i = 0; i < anchorIds.Length; i++)
        {
            //print("anchor #:" + anchorIds[i]);

           // GameObject icon;
            //if (!instantiated)
            //{
                print("instantiating");
                GameObject icon = Instantiate(Resources.Load("GemCanvasPrefab")) as GameObject;
                icon.GetComponent<IconInfo>().iconName = anchorIds[i];
                icons.Add(anchorIds[i], icon);
            //}
            //else
            //{
            //    icon = icons[anchorIds[i]];
            //}
            
            WorldAnchor anchor = this.store.Load(anchorIds[i], icon);

            GemBehavior gb = icon.GetComponent<GemBehavior>();
            gb.SetCanvasText("Testing", "DELETE WHEN UPLOADS FIXED");

            //foreach(string key in lid.iconDonwload.Keys)
            //{
            //    print("key: " + key);
            //}

            //if (lid.iconDonwload.ContainsKey(anchorIds[i]))
            //{
            //    //string[] info = lid.iconDonwload[anchorIds[i]];
            //    //GemBehavior gb = icon.GetComponent<GemBehavior>();
            //    //print("Text:" + info[1] + "title: " + info[0]);
            //    ////print("gb" + gb.ToString());
            //    //gb.SetCanvasText(info[1], info[0]);
            //}
            //GemBehavior gb = icon.GetComponent<GemBehavior>();
            //gb.SetCanvasText("TESTING", "DELETE WHEN UPLOADS ARE FIXED");


            //print("loaded anchor position: " + anchor.transform.position);
            //print("new gameobject position: " + icon.transform.position);
        }

        //instantiated = true;
    }
}
