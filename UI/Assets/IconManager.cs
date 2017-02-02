using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using UnityEngine.Windows.Speech;
using System.Linq;

public class IconManager : MonoBehaviour {

    WorldAnchorStore store;
    Photographer photographer;

    // Use this for initialization
    void Start () {
       
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
        photographer = GameObject.Find("Main Camera").GetComponent<Photographer>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("c"))
        {
            print("cleared");
            this.store.Clear();
        }
        if (Input.GetKeyDown("u"))
        {
            print("uploading...");
            GameObject.Find("Main Camera").GetComponent<SpeechManager>().PerformImageUpload();
        }
    }

    public void SaveAnchor(GameObject go)
    {
        {
            WorldAnchor anchor = go.GetComponent<WorldAnchor>();
            print("anchor label: " + anchor);
            if (anchor == null)
            {
                anchor = go.AddComponent<WorldAnchor>();

                // Adding makes anchoring fail on hololens

                //if (anchor.isLocated)
                //{
                string iconName = go.GetComponent<IconInfo>().iconName;
                print("saving :" + iconName + " @" +anchor.transform.position);
                this.store.Save(iconName, anchor);
                photographer.TakePicture(iconName, true);  
               // }
                //else
                //{
                //    anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
                //}
            }
        }
    }

    public void DeleteAnchor(GameObject go)
    {
        print("deleting" + go);
        this.store.Delete(go.GetComponent<IconInfo>().iconName);
        DestroyImmediate(go.GetComponent<WorldAnchor>());
    }

    public void PlaceBox(Vector3 vect)
    {
        
        GameObject icon = Instantiate(Resources.Load("GemCanvasPrefab")) as GameObject;
        print("ICON" + icon);
        icon.transform.position = vect;

        string iconName = "icon_" + System.DateTime.Now.ToString("MMddyyHmmssfff");
        icon.GetComponent<IconInfo>().iconName = iconName;
        print("instantiating " + iconName + " at " + vect);

        SaveAnchor(icon);
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

    private void Anchor_OnTrackingChanged(WorldAnchor anchor, bool located)
    {
        anchor.gameObject.SetActive(located);
    }
}
