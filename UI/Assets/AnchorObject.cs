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
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
        SaveAnchor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SaveAnchor()
    {
        if (!this.savedRoot)
        {
            WorldAnchor anchor = gameObject.AddComponent<WorldAnchor>();
            this.savedRoot = this.store.Save(iconName, anchor);
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
                WorldAnchor anchor = this.store.Load(anchorIds[i], gameObject);
                break;
            }
        }


    }
}
