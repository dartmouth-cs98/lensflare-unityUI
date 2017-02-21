using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Unity;
using UnityEngine.Networking;

public class UploadImages : MonoBehaviour
{
    const string bucketName = "lensflare-files";
    const string server_url = "http://lensflare-server.herokuapp.com";
    const string signed_url_endpoint = "/sign-s3-photos";
    string[] localFilePaths;

    // for using callbacks in Unity
    // MODIFY THIS TO CHANGE THE CALLBACK TYPE (if adding arguments, need to pass those down as well)
    public delegate bool GenericDelegate(); // boolean return type, no arguments
    private GenericDelegate genDel;

    public void Start()
    {
    }
    public void StartUploadImages(string[] localFilePaths, string[] s3FilePaths, string userEmail, string spaceName, GenericDelegate cb)
    {
        StartCoroutine(Upload(localFilePaths, s3FilePaths, userEmail, spaceName, cb));
    }


    IEnumerator WaitForRequest(WWW www, string mode, GenericDelegate cb)
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
                    
                    //Dictionary<string, string> headers = new Dictionary<string, string>();
                    //string sUrl = FileCollection.files[j].signedUrl;
                    //string head = sUrl.Substring(sUrl.IndexOf("=") + 1);
                    //var heads = head.Split('&');

                    print(FileCollection.files[j].signedUrl);
                    UnityWebRequest req = UnityWebRequest.Put(FileCollection.files[j].signedUrl, imageToUpload);
                    req.SetRequestHeader("Content-Type", "");
                    yield return req.Send();

                    if (req.isError)
                    {
                        print(req.error);
                    }
                    else
                    {
                        print(req.responseCode);
                        print("Upload worked");

                      
                        if (File.Exists(localFilePaths[j]))
                        {
                            print("Deleting " + localFilePaths[j]);
                            File.Delete(localFilePaths[j]);
                        }

                        genDel = cb;
                        genDel();
                    }

                    //headers["AWSAccessKeyId"] = heads[0];
                    //headers["Expires"] = heads[1].Substring(heads[1].IndexOf("=") + 1);
                    //headers["Signature"] = heads[2].Substring(heads[2].IndexOf("=") + 1) + "&" + heads[3];
                    //headers["authorization"] = "AWS " + heads[0] + ":" + heads[2].Substring(heads[2].IndexOf("=") + 1) + "&" + heads[3];
                    ////headers["Authorization"] = "AWS " + heads[0] + ":" + heads[2].Substring(heads[2].IndexOf("=") + 1) + "&" + heads[3];
                    ////print(headers["AWSAccessKeyId"]);
                    ////print(headers["Expires"]);
                    ////print(headers["Signature"]);
                    //WWW post = new WWW(FileCollection.files[j].signedUrl, imageToUpload, headers);
                    //StartCoroutine(WaitForRequest(post, "IndivudalImage"));

                }
            }
            else
            {
                print(www.text);

                // passed callback
            }
        }
        else
        {
            Debug.Log(www.error);

            // passed callback <-- should this be called in the error case as well?
            genDel = cb;
            genDel();
        }

    }

    IEnumerator Upload(string[] userFilePaths, string[] s3FilePaths, string userEmail, string spaceName, GenericDelegate cb)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] jsonBytes = encoding.GetBytes(ConstructRequestJson(userEmail, spaceName, s3FilePaths));
        localFilePaths = userFilePaths;
        
        //HttpWebRequest signedUrlRequest = (HttpWebRequest)WebRequest.Create(server_url + signed_url_endpoint);
        //signedUrlRequest.ContentType = "application/json";
        //signedUrlRequest.Method = "POST";
        //signedUrlRequest.ContentLength = jsonBytes.Length;
        //signedUrlRequest.GetRequestStream().Write(jsonBytes, 0, jsonBytes.Length);

        //byte[] arr = new byte[s3FilePaths.Length * string.];
        //for (int i = 0; i < s3FilePaths.Length; i++)
        //{
        //    arr[i] = encoding.GetBytes(s3FilePaths[i]);
        //}
        //WWWForm wwwForm = new WWWForm();
        //wwwForm.AddField("email", userEmail);
        //wwwForm.AddField("space", spaceName);
        //wwwForm.AddBinaryData("files", arr);
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["content-type"] = "application/json";
        WWW www = new WWW(server_url + signed_url_endpoint, jsonBytes, headers);
        StartCoroutine(WaitForRequest(www, "PostImage", cb));

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
