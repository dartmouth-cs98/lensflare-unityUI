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
    Boolean downloadDone = false;

    Dictionary<string, Item> iconDonwload;

    public Dictionary<string, Item> GetIconDownload(){
        return iconDonwload;
    }

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
	
    public Boolean isDownloadDone()
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

        //        HttpWebRequest signedUrlRequest = (HttpWebRequest)WebRequest.Create(server_url + signed_url_endpoint);
        //        signedUrlRequest.ContentType = "application/json";
        //        signedUrlRequest.Method = "GET";

        //        HttpWebResponse signedUrlResponse = (HttpWebResponse)signedUrlRequest.GetResponse();
        //        print(signedUrlResponse.StatusDescription);
        //        WebResponse webResponse = signedUrlRequest.GetResponse();
        //        Stream dataStream = webResponse.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);
        //        string response = reader.ReadToEnd();
        //        print(response);

        yield return www;

        if (www.error == null)
        {
            print("Request Successful");

            string detections = www.text;
            Debug.Log(detections);
            Space parsedResponse = JsonUtility.FromJson<Space>(detections);
            iconDonwload = new Dictionary<string, Item>();

            //THIS IS PRINTING NOTHING, 
            print(parsedResponse.anchors);
            if (parsedResponse.anchors != null)
            {
                StartCoroutine(DownloadWorldAnchor(parsedResponse.anchors));
            }
            Item[] items = parsedResponse.items;
            for (int j = 0; j < items.Length; j++)
            {

                Item item = items[j];
                item.iconName = item.url.Split('/')[3].Split('.')[0];

                if (!iconDonwload.ContainsKey(item.iconName))
                {
                    iconDonwload.Add(item.iconName, item);
                }

            }
            downloadDone = true; 
        }
        else
        {
            if (www.error.Contains("401"))
            {
                // alert the user that the token no longer works
                print("Token is not valid");
                SceneManager.LoadScene("PairingScene");
            }
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

        //HttpWebRequest signedUrlRequest = (HttpWebRequest)WebRequest.Create(server_url + signed_url_endpoint);
        //signedUrlRequest.ContentType = "application/json";
        //signedUrlRequest.Method = "GET";

        //HttpWebResponse signedUrlResponse = (HttpWebResponse)signedUrlRequest.GetResponse();
        //print(signedUrlResponse.StatusDescription);
        //WebResponse webResponse = signedUrlRequest.GetResponse();
        //Stream dataStream = webResponse.GetResponseStream();
        //StreamReader reader = new StreamReader(dataStream);
        //string response = reader.ReadToEnd();
        //print(response);
        //var parsedResponse = JSON.Parse(response);
        //JSONNode spaces = parsedResponse["local"]["spaces"];
        //for (int i = 0; i < spaces.Count; i++)
        //{
        //    JSONNode items = spaces[i]["items"];
        //    for (int j = 0; j < items.Count; j++)
        //    {
        //        print(items[j]["title"]);
        //    }
        //}
        //reader.Close();
        //signedUrlResponse.Close();

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
        //print("deserialized anchor count: " + deserializedTransferBatch.anchorCount);

        if (deserializedTransferBatch.anchorCount == 0)
        {
            print("No Anchors going to placement scene");
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
            return;
        }
        string[] anchorIds = deserializedTransferBatch.GetAllIds();

        for (int i = 0; i < anchorIds.Length; i++)
        {
            print("anchor #:" + anchorIds[i]);
            GameObject icon = Instantiate(Resources.Load("MediaGemPrefab")) as GameObject;
            icon.GetComponent<IconInfo>().info.iconName = anchorIds[i];
            //WorldAnchor anchor = this.store.Load(anchorIds[i], icon);
            deserializedTransferBatch.LockObject(anchorIds[i], icon);
        }
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
