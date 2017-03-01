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
    GameObject[] iconInstances;

    // Use this for initialization
    void Start () {

        print("LOADING ANCHORSSSSSS");
        surfaceObserver = new SurfaceObserver();
        surfaceObserver.SetVolumeAsAxisAlignedBox(Vector3.zero, new Vector3(3, 3, 3));

        lid = GetComponent<LoadIconData>();
        icons = new Dictionary<string, GameObject>();
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);

        //StartCoroutine(UpdateLoop());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSurfaceChanged(SurfaceId surfaceId, SurfaceChange changeType, Bounds bounds, System.DateTime updateTime)
    {
        ////print("surface changed");
    }

    IEnumerator UpdateLoop(float count)
    {
        yield return new WaitForSeconds(count);
        AnchorStoreLoaded(this.store);
        //var wait = new WaitForSeconds(1.0f);
        //while (true)
        //{
        //    surfaceObserver.Update(OnSurfaceChanged);
        //    yield return wait;
        //}
    }

    public WorldAnchorStore GetAnchorStore()
    {
        return this.store;
    }

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;

        string[] anchorIds = this.store.GetAllIds();

        if (!lid.isDownloadDone())
        {
            StartCoroutine(UpdateLoop(0.2f));
            return;
        }
        print("loadanchoricons " + anchorIds.Length + " " + instantiated);

        if (iconInstances == null) iconInstances = new GameObject[anchorIds.Length];

        for (int i = 0; i < anchorIds.Length; i++)
        {
            print("instantiating");

            if (iconInstances[i] == null ) iconInstances[i] = Instantiate(Resources.Load("MediaGemPrefab")) as GameObject;

            GameObject icon = iconInstances[i];
            if (icon == null)
            {
                print("icon is null");
            }

//            icon.GetComponent<IconInfo>().iconName = anchorIds[i];
            if (!icons.ContainsKey(anchorIds[i])) icons.Add(anchorIds[i], icon);

            WorldAnchor anchor = this.store.Load(anchorIds[i], icon);

            // Prints
            print("Loaded anchor" + anchorIds[i]);
            print(lid.GetIconDownload().ToString());
            foreach (string key in lid.GetIconDownload().Keys)
            {
                print(key);
            }
            print("Found ID in dict = " + lid.GetIconDownload().ContainsKey(anchorIds[i]));

            if (lid.GetIconDownload().ContainsKey(anchorIds[i]))
            {
                LoadIconData.Item info = lid.GetIconDownload()[anchorIds[i]];
                icon.GetComponent<IconInfo>().info = info;
                GemBehavior gb = icon.GetComponent<GemBehavior>();
                print("Text:" + info.text + "title: " + info.title);
                print("gb" + gb.ToString());
                gb.SetCanvasText(info.title, info.text);
            }
        }
        StartCoroutine(UpdateLoop(60.0f));

    }
}
