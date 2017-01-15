using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;


public class AnchorObject : MonoBehaviour
{

    WorldAnchorStore store;
    public string iconName = "iconName";
    public bool savedRoot = false;

    // Use this for initialization
    void Start()
    {
        print("Start POS:" + gameObject.transform.position);
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            print("F pressed");
            MoveAnchor(new Vector3(0.2f, 0.5f, 1.2f));
            print("moved to:" + gameObject.transform.position.ToString());
        }

        if (Input.GetKeyDown("c"))
        {
            print("cleared");
            this.store.Clear();
        }

    }

    private void SaveAnchor()
    {
        if (!this.savedRoot)
        {
            print("Saving root");
  
            WorldAnchor anchor = gameObject.GetComponent<WorldAnchor>();
            if (anchor == null)
            {
                print("Anchor is null");
                anchor = gameObject.AddComponent<WorldAnchor>();
                anchor.transform.position = new Vector3(0.3f, 0.3f, 1.1f);
                //if (anchor.isLocated)
                //{
                print("position of first saved anchor:" + anchor.transform.position);
                this.store.Save(iconName, anchor);

                    
                print("loaded position save: " + this.store.Load(iconName, gameObject).transform.position);
                //}
                //else
                //{
                //    anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
                //}
            }

        }
    }

    private void MoveAnchor(Vector3 vect)
    {
        this.store.Delete(iconName);
        DestroyImmediate(gameObject.GetComponent<WorldAnchor>());

        print("deleted anchor count:" + this.store.anchorCount);
        

        gameObject.transform.position = vect;
        WorldAnchor anchor = gameObject.AddComponent<WorldAnchor>();
        print("new anchor position: " + anchor.transform.position);
        if (anchor.isLocated)
        {
            this.savedRoot = this.store.Save(iconName, anchor);

            print("loaded position: " + this.store.Load(iconName, gameObject).transform.position);
            print("new saved anchor count:" + this.store.anchorCount);
        }
        else
        {
            anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
        }

        
    }

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;

        string[] anchorIds = this.store.GetAllIds();
        for (int i = 0; i < anchorIds.Length; i++)
        {
            if (anchorIds[i] == iconName)
            {
                print("Found iconName");
                WorldAnchor anchor = this.store.Load(anchorIds[i], gameObject);
                print("loaded anchor position: " + anchor.transform.position);
                this.savedRoot = true;
                break;
                
            }
        }

        print("loaded pos:" + gameObject.transform.position.ToString());
        SaveAnchor();
    }

    private void Anchor_OnTrackingChanged(WorldAnchor anchor, bool located)
    {
        anchor.gameObject.SetActive(located);
        //if (located)
        //{
        //    this.store.Save(iconName, anchor);
        //    anchor.OnTrackingChanged -= Anchor_OnTrackingChanged;
        //}
    }
}
