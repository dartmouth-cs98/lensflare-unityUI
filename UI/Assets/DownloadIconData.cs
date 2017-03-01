//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.VR.WSA.Sharing;

//public class DownloadIconData : MonoBehaviour {

//    private int retryCount = 10;
//    List<byte> anchorByteBuffer = new List<byte>();
//    public WorldAnchorTransferBatch transferBatch;


//    // Use this for initialization
//    void Start () {
//        LoadIconData iconData = gameObject.GetComponent<LoadIconData>();
//        //while (!iconData.isDownloadDone())
//        //{
//        //    print("waiting");
//        //}
//        //StartCoroutine(DownloadWorldAnchor("https://s3.amazonaws.com/lensflare-files/anchors_58ac9701dea544a4fbbc9b3b_1487717841"));
//        print(iconData.getAnchorUrl());
//        StartCoroutine(DownloadWorldAnchor(iconData.getAnchorUrl()));
//    }

//    // Update is called once per frame
//    void Update () {
		
//	}

//    IEnumerator DownloadWorldAnchor(string url)
//    {
//        WWW www = new WWW(url);
//        yield return www;
//        byte[] anchors = www.bytes;
//        print(anchors.Length);
//        ImportWorldAnchor(anchors);
//    }

//    private void ImportWorldAnchor(byte[] importedData)
//    {
//        WorldAnchorTransferBatch.ImportAsync(importedData, OnImportComplete);
//    }

//    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
//    {
//        //print("deserialized anchor count: " + deserializedTransferBatch.anchorCount);

//        if (deserializedTransferBatch.anchorCount == 0)
//        {
//            print("No Anchors going to placement scene");
//            SceneManager.LoadScene("PlacementScene");
//            return;
//        }

//        if (completionReason != SerializationCompletionReason.Succeeded || deserializedTransferBatch.anchorCount == 0)
//        {
//            Debug.Log("Failed to import: " + completionReason.ToString());
//            if (retryCount > 0)
//            {
//                retryCount--;

//                print("retrying with: " + anchorByteBuffer.ToArray().Length);
//                WorldAnchorTransferBatch.ImportAsync(anchorByteBuffer.ToArray(), OnImportComplete);
//            }
//            return;
//        }
//        string[] anchorIds = deserializedTransferBatch.GetAllIds();

//        for (int i = 0; i < anchorIds.Length; i++)
//        {
//            print("anchor #:" + anchorIds[i]);
//            GameObject icon = Instantiate(Resources.Load("MediaGemPrefab")) as GameObject;
//            icon.GetComponent<IconInfo>().info.iconName = anchorIds[i];
//            //WorldAnchor anchor = this.store.Load(anchorIds[i], icon);
//            deserializedTransferBatch.LockObject(anchorIds[i], icon);
//        }
//    }
//}
