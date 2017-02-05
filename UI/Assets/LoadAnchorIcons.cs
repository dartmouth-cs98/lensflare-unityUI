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

	// Use this for initialization
	void Start () {
        surfaceObserver = new SurfaceObserver();
        surfaceObserver.SetVolumeAsAxisAlignedBox(Vector3.zero, new Vector3(3, 3, 3));
        
        StartCoroutine(UpdateLoop());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSurfaceChanged(SurfaceId surfaceId, SurfaceChange changeType, Bounds bounds, System.DateTime updateTime)
    {
        print("surface changed");
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
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
        print(anchorIds.Length);

        for (int i = 0; i < anchorIds.Length; i++)
        {
            print("anchor #:" + anchorIds[i]);
            GameObject icon = Instantiate(Resources.Load("GemCanvasPrefab")) as GameObject;
            icon.GetComponent<IconInfo>().iconName = anchorIds[i];
            WorldAnchor anchor = this.store.Load(anchorIds[i], icon);

            // Manually set location?

            print("loaded anchor position: " + anchor.transform.position);
            print("new gameobject position: " + icon.transform.position);
        }
    }
}
