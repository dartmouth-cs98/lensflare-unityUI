using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using UnityEngine.Windows.Speech;
using System.Linq;

public class IconManager : MonoBehaviour {

    WorldAnchorStore store;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start () {
       
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);

        // Testing purposes
        //keywords.Add("Move", () =>
        //{
        //    print("moved");
        //    MoveAnchor(new Vector3(0.2f, 0.5f, 1.2f));
        //});

        //keywords.Add("Clear", () =>
        //{
        //    print("cleared");
        //    this.store.Clear();
        //});

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Testing
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("c"))
        {
            print("cleared");
            this.store.Clear();
        }
    }

    public void SaveAnchor(string iconName, GameObject go)
    {
        {
            WorldAnchor anchor = go.GetComponent<WorldAnchor>();
            print("anchor label: " + anchor);
            if (anchor == null)
            {
                anchor = go.AddComponent<WorldAnchor>();

                //if (anchor.isLocated)
                //{
                    print("saving :" + iconName);
                    this.store.Save(iconName, anchor);
              //  }
                //else
                //{
                //    anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
                //}
            }
        }
    }

    private void MoveAnchor(string iconName, GameObject go, Vector3 vect)
    {

        if (go.GetComponent<WorldAnchor>() == null)
        {
            go.transform.position = vect;
            SaveAnchor(iconName, go);
        }
        else
        {
            this.store.Delete(iconName);
            DestroyImmediate(go.GetComponent<WorldAnchor>());

            go.transform.position = vect;
            WorldAnchor anchor = go.AddComponent<WorldAnchor>();
            if (anchor.isLocated)
            {
                print("saving :" + iconName);
                this.store.Save(iconName, anchor);
            }
            else
            {
                anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
            }
        }
    }

    public void PlaceBox(Vector3 vect)
    {
        GameObject icon = Instantiate(Resources.Load("IconPrefab")) as GameObject;
        icon.transform.position = vect;

        string iconName = "icon_" + System.DateTime.Now.ToString("MMddyyHmmssfff");
        print("instantiating " + iconName + " at " + vect);

        SaveAnchor(iconName, icon);
    }

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;

        string[] anchorIds = this.store.GetAllIds();
        print(anchorIds.Length);

        for (int i = 0; i < anchorIds.Length; i++)
        {
            print("anchor #:" + anchorIds[i]);
            GameObject icon = Instantiate(Resources.Load("IconPrefab")) as GameObject;
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
