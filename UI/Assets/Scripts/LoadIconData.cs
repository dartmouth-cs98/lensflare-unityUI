using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using SimpleJSON;


public class LoadIconData : MonoBehaviour {


    const string bucketName = "lensflare-files";
    const string server_url = "http://lensflare-server.herokuapp.com/getSpacesWithToken?token=58a9ddfcccd5a5001c1ccb00";
    Boolean downloadDone = false; 

    Dictionary<string, string[]> iconDonwload;

    public Dictionary<string, string[]> GetIconDownload(){
        return iconDonwload;
    }

    // Use this for initialization
    void Start () {
        print("About to Download Anchor Data");
        InvokeRepeating("download", 0.0f, 10.0f);
        //download();
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
            var parsedResponse = JSON.Parse(detections);
            JSONNode spaces = parsedResponse["local"]["spaces"];
            iconDonwload = new Dictionary<string, string[]>();
            for (int i = 0; i < spaces.Count; i++)
            {
                JSONNode items = spaces[i]["items"];
                for (int j = 0; j < items.Count; j++)
                {
                    string url = items[j]["url"].ToString();
                    string iconName = url.Split('/')[3].Split('.')[0];
                    //print(iconName);

                    string[] info = new string[] { items[j]["title"], items[j]["text"] };

                    if (!iconDonwload.ContainsKey(iconName))
                    {
                        iconDonwload.Add(iconName, info);
                    }
                    //print(items[j]["url"]);
                }
            }
            downloadDone = true; 
        }
        else
        {
            Debug.Log(www.error);
        }
        
    }
    string getUTCTime()
    {
        System.Int32 unixTimestamp = (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        return unixTimestamp.ToString();
    }
    public void download()
    {

        WWW www = new WWW(server_url + "&t=" + getUTCTime());
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
}
