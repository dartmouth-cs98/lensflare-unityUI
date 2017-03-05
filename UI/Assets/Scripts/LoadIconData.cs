using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Sharing;

public class LoadIconData : MonoBehaviour {

    private int retryCount = 10;
    List<byte> anchorByteBuffer = new List<byte>();
    public WorldAnchorTransferBatch transferBatch;

    const string bucketName = "lensflare-files";
    const string server_url = "http://lensflare-server.herokuapp.com/getSpaceWithToken?token={0}&t={1}";
    bool downloadDone = false;
    bool iconsAnchored = false;

    // Track icon info to immediately set when gems are instantiated
    Dictionary<string, Item> iconInfo = new Dictionary<string, Item>();

    // Track gems so updated info can be set immediately on existing gems
    Dictionary<string, GameObject> icons = new Dictionary<string, GameObject>();


    // Use this for initialization
    void Start () {
        print("About to Download Anchor Data");
        InvokeRepeating("download", 0.0f, 10.0f);
        download();

        string deviceToken = PlayerPrefs.GetString("device_token", "");
        Debug.Log(deviceToken);
        if (deviceToken == null || deviceToken.Equals(""))
        {
            SceneManager.LoadScene("PairingScene");
            return;
        }
    }
	
    public bool isDownloadDone()
    {
        return downloadDone;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("n"))
        {
            download();
        }
    }

    IEnumerator WaitForRequest(WWW www, string mode)
    {
        print("In Download CoRoutine");
        yield return www;

        if (www.error == null)
        {
            print("Request Successful");

            string detections = www.text;
            Debug.Log(detections);
            Space parsedResponse = JsonUtility.FromJson<Space>(detections);
            iconInfo.Clear();

            print(parsedResponse.anchors.Length);
            if (parsedResponse.anchors == null || parsedResponse.anchors.Length == 0)
            {
                // TODO error out
                print("spatial mapping download error");
            } 
            else if (iconsAnchored)
            {
                print("anchors already loaded");    
            }
            else
            {
                iconsAnchored = true;
                StartCoroutine(DownloadWorldAnchor(parsedResponse.anchors));
            }

            Item[] items = parsedResponse.items;
            for (int j = 0; j < items.Length; j++)
            {

                Item item = items[j];
                item.iconName = item.url.Split('/')[3].Split('.')[0];

                if (icons.ContainsKey(item.iconName))
                {
                    SetInfo(icons[item.iconName], item);
                }
                else
                {
                    iconInfo.Add(item.iconName, item);
                }
            }
        }
        else
        {
            if (www.error.Contains("401") || www.error.Contains("Access is denied."))
            {
                // alert the user that the token no longer works
                print("Token is not valid");
                SceneManager.LoadScene("PairingScene");
            }
            print("error");
            Debug.Log(www.error);
        }
        
    }
    public static string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }
    public void download()
    {

        string deviceToken = PlayerPrefs.GetString("device_token","");

        if (deviceToken == null || deviceToken.Equals(""))
        {
            print("Loading pairing scene");
            SceneManager.LoadScene("PairingScene");
            return;
        }

        Debug.Log("device token: " + deviceToken);
        WWW www = new WWW(String.Format(server_url, deviceToken, getUTCTime()));
        StartCoroutine(WaitForRequest(www, "GetSpaces"));
    }


    IEnumerator DownloadWorldAnchor(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        byte[] anchors = www.bytes;
        print(anchors.Length);
        ImportWorldAnchor(anchors);
    }

    private void ImportWorldAnchor(byte[] importedData)
    {
        WorldAnchorTransferBatch.ImportAsync(importedData, OnImportComplete);
    }

    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
    {
        if (deserializedTransferBatch.anchorCount == 0)
        {
            print("No Anchors going to placement scene");
            gameObject.GetComponent<LoadingSpeechManager>().ClearAllGems();

            SceneManager.LoadScene("PlacementScene");
            return;
        }

        if (completionReason != SerializationCompletionReason.Succeeded || deserializedTransferBatch.anchorCount == 0)
        {
            Debug.Log("Failed to import: " + completionReason.ToString());
            if (retryCount > 0)
            {
                retryCount--;

                print("retrying with: " + anchorByteBuffer.ToArray().Length);
                WorldAnchorTransferBatch.ImportAsync(anchorByteBuffer.ToArray(), OnImportComplete);
            }
            else
            {
                iconsAnchored = false;
            }
            return;
        }
        string[] anchorIds = deserializedTransferBatch.GetAllIds();

        for (int i = 0; i < anchorIds.Length; i++)
        {
            print("load icon anchor #:" + anchorIds[i]);
            GameObject icon = Instantiate(Resources.Load("MediaGemPrefab")) as GameObject;

            Item info = iconInfo[anchorIds[i]];
            SetInfo(icon, info);
            icons[info.iconName] = icon;

            deserializedTransferBatch.LockObject(info.iconName, icon);
        }
        iconsAnchored = true;
        downloadDone = true;
    }

    private void SetInfo(GameObject icon, Item info)
    {
        icon.GetComponent<IconInfo>().info = info;
        print("Text:" + info.text + "title: " + info.title);
    }

    [Serializable]
    public class Item
    {
        public string title;
        public string text;
        public string url;
        public string iconName;
        public Media media;
    }

    [Serializable]
    public class Space
    {
        public string anchors;
        public Item[] items;
    }

    [Serializable]
    public class Media
    {
        public string type;
        public int height;
        public int width;
        public string media_url; 
    }
}
