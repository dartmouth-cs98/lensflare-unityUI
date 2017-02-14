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
    const string signed_url_endpoint = "/sign-s3";
    string[] localFilePaths;
    byte[] bArrayToUpload;

    // for using callbacks in Unity
    // MODIFY THIS TO CHANGE THE CALLBACK TYPE (if adding arguments, need to pass those down as well)
    public delegate bool GenericDelegate(); // boolean return type, no arguments
    private GenericDelegate genDel;

    public void Start()
    {
    }

    public void StartUploadFiles(string[] localFilePaths, string[] s3FilePaths, string userEmail, string spaceName, GenericDelegate cb)
    {
        localFilePaths = userFilePaths;
        StartCoroutine(Upload(s3FilePaths, userEmail, spaceName, "PostFiles", cb));
    }

    public void StartUploadByteArrays(byte[] bArray, string[] s3FilePaths, string userEmail, string spaceName, GenericDelegate cb)
    {
        bArrayToUpload = bArray;
        StartCoroutine(Upload(s3FilePaths, userEmail, spaceName, "PostBytes", cb));
    }


    IEnumerator WaitForRequest(WWW www, string mode, GenericDelegate cb)
    {
        yield return www;

        if (www.error == null)
        {
            if (mode.Equals("PostFiles"))
            {
                string resp = www.text;
                FileCollection FileCollection = null;

                try
                {
                    FileCollection = JsonUtility.FromJson<FileCollection>(resp);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                byte[] fileToUpload = null;
                for (int j = 0; j < FileCollection.files.Length; j++)
                {
                    try
                    {
                        fileToUpload = File.ReadAllBytes(localFilePaths[j]);
                    }
                    catch (FileNotFoundException e)
                    {
                        Debug.Log(e);
                        throw e;
                    }

                    print(FileCollection.files[j].signedUrl);
                    UnityWebRequest req = UnityWebRequest.Put(FileCollection.files[j].signedUrl, fileToUpload);
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
                }
            }
            else if (mode.equals("PostBytes"))
            {
                string resp = www.text;
                FileCollection FileCollection = null;

                try
                {
                    FileCollection = JsonUtility.FromJson<FileCollection>(resp);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                print(FileCollection.files[j].signedUrl);
                UnityWebRequest req = UnityWebRequest.Put(FileCollection.files[j].signedUrl, bArrayToUpload);
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

                    genDel = cb;
                    genDel();
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

            // passed callback <-- should this be called in the error case as well?
            genDel = cb;
            genDel();
        }

    }

    IEnumerator Upload(string[] s3FilePaths, string userEmail, string spaceName, string mode, GenericDelegate cb)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] jsonBytes = encoding.GetBytes(ConstructRequestJson(userEmail, spaceName, s3FilePaths));
        
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["content-type"] = "application/json";
        WWW www = new WWW(server_url + signed_url_endpoint, jsonBytes, headers);
        StartCoroutine(WaitForRequest(www, mode, cb));

        yield return 0;
    }

    // shouldn't need to be changed?
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
