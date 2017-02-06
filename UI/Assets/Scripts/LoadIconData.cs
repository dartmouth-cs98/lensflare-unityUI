﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using SimpleJSON;


public class LoadIconData : MonoBehaviour {


    const string bucketName = "lensflare-files";
    const string server_url = "http://lensflare-server.herokuapp.com";
    const string signed_url_endpoint = "/getSpacesUnauth?email=nick@moolenijzer.com";

    public Dictionary<string, string[]> iconDonwload;

    // Use this for initialization
    void Start () {
        download();
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
            
        }
        else
        {
            Debug.Log(www.error);
        }
        
    }

    public void download()
    {

        WWW www = new WWW(server_url + signed_url_endpoint);
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