using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


public class LoadIconData : MonoBehaviour {

    const string bucketName = "lensflare-files";
    const string server_url = "http://lensflare-server.herokuapp.com";
    const string signed_url_endpoint = "/getSpacesUnauth?email=nick@moolenijzer.com";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("n"))
        {
            Console.WriteLine("HERE");
            download();
        }
    }

    public void download()
    {
        //ASCIIEncoding encoding = new ASCIIEncoding();
        //byte[] jsonBytes = encoding.GetBytes(ConstructRequestJson(userEmail, spaceName, s3FilePaths));

        HttpWebRequest signedUrlRequest = (HttpWebRequest)WebRequest.Create(server_url + signed_url_endpoint);
        signedUrlRequest.ContentType = "application/json";
        signedUrlRequest.Method = "GET";

        HttpWebResponse signedUrlResponse = (HttpWebResponse)signedUrlRequest.GetResponse();
        print(signedUrlResponse.StatusDescription);
        WebResponse webResponse = signedUrlRequest.GetResponse();
        Stream dataStream = webResponse.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string response = reader.ReadToEnd();
        print(response);
        reader.Close();
        signedUrlResponse.Close();

    }
}
