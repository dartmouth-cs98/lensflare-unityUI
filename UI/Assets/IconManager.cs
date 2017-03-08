using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using System.Text;
using UnityEngine.VR.WSA.Sharing;

public class IconManager : MonoBehaviour {

    const string ADD_ANCHOR_ENDPOINT = "http://lensflare-server.herokuapp.com/addAnchors";

    WorldAnchorStore store;
    public WorldAnchorTransferBatch transferBatch;
    Photographer photographer;
    List<byte> anchorByteBuffer = new List<byte>();
    private int retryCount = 10;

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

        if (Input.GetKeyDown("w"))
        {
            MakeTransferBatch();
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

                string iconName = go.GetComponent<IconInfo>().info.iconName;
                print("saving :" + iconName + " @" +anchor.transform.position);
                this.store.Save(iconName, anchor);

                photographer.TakePicture(iconName, true);  
            }
        }
    }

    public void MakeTransferBatch()
    {
        print("Anchor store is " + this.store.ToString());
        transferBatch = new WorldAnchorTransferBatch();
        GameObject[] gems = GameObject.FindGameObjectsWithTag("GemCanvas");
        print("number of gems:  " + gems.Length);
        foreach(GameObject gem in gems)
        {
            string name = gem.GetComponent<IconInfo>().info.iconName;
            if (name != null && name != "")
            {
                WorldAnchor anchor = this.store.Load(name, gem);

                if (anchor != null)
                {
                    print("adding anchor with name" + name + " gem " + gem.ToString());
                    this.transferBatch.AddWorldAnchor(name, anchor);
                }
            }
        }

        //print("export anchor count: " + this.transferBatch.GetAllIds().Length);
        WorldAnchorTransferBatch.ExportAsync(this.transferBatch, OnExportDataAvailable, OnExportComplete);
    }

    private void OnExportDataAvailable(byte[] data)
    {
        // Send the bytes to the client.  Data may also be buffered.
        anchorByteBuffer.AddRange(data);
    }

    private void OnExportComplete(SerializationCompletionReason completionReason)
    {
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            // If we have been transferring data and it failed, 
            // tell the client to discard the data
            print("Export failed, retrying.");
            MakeTransferBatch();
        }
        else
        {
            // Tell the client that serialization has succeeded.
            // The client can start importing once all the data is received.
            string token = PlayerPrefs.GetString("device_token");
            print("device token is " + token);
      
            string path = "anchors_" + token + "_" + LoadIconData.getUTCTime();
            Camera.main.GetComponent<UploadImages>().StartUploadByteArray(anchorByteBuffer.ToArray(), path, token, (url) =>
            {
                print("PATH: " + path);
                StartCoroutine(SaveAnchors(path, token, url)); 
                // save in the db
                return true; 
            });
        }
    }

    IEnumerator SaveAnchors(string path, string token, string url)
    {
        print("In Save Anchors");
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] jsonBytes = encoding.GetBytes("{\"token\":\"" + token + "\", \"anchors\":\"" + url + "\"}");

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["content-type"] = "application/json";
        WWW www = new WWW(ADD_ANCHOR_ENDPOINT, jsonBytes, headers);

        yield return www;

        if(www.error != null)
        {
            Debug.Log(www.error);
        }
    }

    public void DeleteAnchor(GameObject go)
    {
        print("deleting anchor" + go);
        print(go.GetComponent<IconInfo>());

        print(go.GetComponent<IconInfo>().info);

        print(go.GetComponent<IconInfo>().info.iconName);
        this.store.Delete(go.GetComponent<IconInfo>().info.iconName);
 
        DestroyImmediate(go.GetComponent<WorldAnchor>());
    }

    public void PlaceBox(Vector3 vect)
    {
        
        GameObject icon = Instantiate(Resources.Load("MediaGemPrefab")) as GameObject;
        icon.transform.position = vect;

        string iconName = "icon_" + System.DateTime.Now.ToString("MMddyyHmmssfff");
        print("ICON: " + iconName);

        LoadIconData.Item item = new LoadIconData.Item();
        item.iconName = iconName;
        icon.GetComponent<IconInfo>().info = item;
        print(icon.GetComponent<IconInfo>().info.iconName);
        SaveAnchor(icon);
    }

    public WorldAnchorStore GetAnchorStore()
    {
        return this.store;
    }

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;
        print("existing anchors:");

        for (int i = 0; i < store.GetAllIds().Length; i++)
        {
            print("id: " + store.GetAllIds()[i]);
        }
    }

    private void Anchor_OnTrackingChanged(WorldAnchor anchor, bool located)
    {
        anchor.gameObject.SetActive(located);
    }
}
