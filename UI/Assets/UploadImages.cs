using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public class UploadImages : MonoBehaviour
{
    const string bucketName = "lensflare-files";
    const string server_url = "http://lensflare-server.herokuapp.com";
    const string signed_url_endpoint = "/sign-s3";
    public void Start()
    {
    }
    public void StartUploadImages(string[] localFilePaths, string[] s3FilePaths, string userEmail, string spaceName)
    {
        StartCoroutine(Upload(localFilePaths, s3FilePaths, userEmail, spaceName));
    }


    IEnumerator WaitForRequest(WWW www, string mode, string[] localFilePaths)
    {
        yield return www;

        if (www.error == null)
        {
            if (mode.Equals("PostImage"))
            {
                string detections = www.text;

                FileCollection FileCollection = null;

                try
                {
                    FileCollection = JsonUtility.FromJson<FileCollection>(detections);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                byte[] imageToUpload = null;
                for (int j = 0; j < FileCollection.files.Length; j++)
                {
                    try
                    {
                        imageToUpload = File.ReadAllBytes(localFilePaths[j]);
                    }
                    catch (FileNotFoundException e)
                    {
                        Debug.Log(e);
                        throw e;
                    }

                    WWW post = new WWW(FileCollection.files[j].signedUrl);
                    WWWForm wwwForm = new WWWForm();
                    wwwForm.AddBinaryData("test", imageToUpload);
                    StartCoroutine(WaitForRequest(www, "IndivudalImage", localFilePaths));

                }
            }
            else
            {
                print(www.text);
            }
        }
        else
        {
            Debug.Log(www.error);
        }

    }

    IEnumerator Upload(string[] localFilePaths, string[] s3FilePaths, string userEmail, string spaceName)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] jsonBytes = encoding.GetBytes(ConstructRequestJson(userEmail, spaceName, s3FilePaths));

        //HttpWebRequest signedUrlRequest = (HttpWebRequest)WebRequest.Create(server_url + signed_url_endpoint);
        //signedUrlRequest.ContentType = "application/json";
        //signedUrlRequest.Method = "POST";
        //signedUrlRequest.ContentLength = jsonBytes.Length;
        //signedUrlRequest.GetRequestStream().Write(jsonBytes, 0, jsonBytes.Length);


        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("test", jsonBytes);
        WWW www = new WWW(server_url + signed_url_endpoint, wwwForm);
        StartCoroutine(WaitForRequest(www, "PostImage", localFilePaths));


        //var client = new Http




        //HttpWebResponse signedUrlResponse = (HttpWebResponse)signedUrlRequest.GetResponse();
        //FileCollection FileCollection = null;

        //try
        //{
        //    FileCollection = JsonUtility.FromJson<FileCollection>(new StreamReader(signedUrlResponse.GetResponseStream()).ReadToEnd());


        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e);
        //}

        //byte[] imageToUpload = null;
        //for (int j = 0; j < FileCollection.files.Length; j++)
        //{
        //    try
        //    {
        //        imageToUpload = File.ReadAllBytes(localFilePaths[j]);
        //    }
        //    catch (FileNotFoundException e)
        //    {
        //        Debug.Log(e);
        //        throw e;
        //    }

        //    HttpWebRequest uploadRequest = (HttpWebRequest)WebRequest.Create(FileCollection.files[j].signedUrl);
        //    uploadRequest.Method = "PUT";
        //    uploadRequest.ContentLength = imageToUpload.Length;
        //    uploadRequest.GetRequestStream().Write(imageToUpload, 0, imageToUpload.Length);

        //    try
        //    {
        //        HttpWebResponse uploadResponse = uploadRequest.GetResponse() as HttpWebResponse;
        //        Debug.Log("File Uploaded");
        //    }
        //    catch (WebException e)
        //    {
        //        Debug.Log("Upload Failed");
        //        Debug.Log(new StreamReader(e.Response.GetResponseStream()).ReadToEnd());
        //    }
        //}

        yield return 0;
    }

    string ConstructRequestJson(string userEmail, string spaceName, string[] s3FilePaths)
    {
        string json = "{" +String.Format("\"email\":\"{0}\", \"space\": \"{1}\", \"files\": [", userEmail, spaceName);
        for (int i = 0; i < s3FilePaths.Length; i++)
        {
            json += "{\"fileName\":\"" + s3FilePaths[i] + "\"}";
            if (i != s3FilePaths.Length - 1)
            {
                json += ",";
            }
        }
        json += "]}";

        return json;
    }



    [Serializable]
    public class FileCollection
    {
        public FileInfo[] files;
    }

    [Serializable]
    public class FileInfo
    {
        public string fileName;
        public string signedUrl;
        public string url;
    }

    [Serializable]
    public class Item
    {
        public string title;
        public string text;
        public string url;
    }

    [Serializable]
    public class Space
    {
        public string name;
        public Item[] items;
    }
}
