using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;
using System.Collections;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using Academy.HoloToolkit.Unity;

public class Placeholder : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public Canvas canvas;
    public bool scanning = false; 

    void Start()
    {

        keywords.Add("Scan", () => {
            OnScan();
        });



        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown("s"))
    //    {
    //        OnScan();
    //    }
    //}
	
    public void OnScan()
    {
        print("Scanning for QR Code");
        if (scanning)
        {
            return;
        }
        scanning = true; 
#if !UNITY_EDITOR
        MediaFrameQrProcessing.Wrappers.ZXingQrCodeScanner.ScanFirstCameraForQrCode(
            result =>
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if (result.Text != null)
                    {
                        PlayerPrefs.SetString("device_token", result.Text);
                        PlayerPrefs.Save();
                        canvas.GetComponentInChildren<Text>().text = "Sucessfully Paired Device";
                        print(result.Text);
                        StartCoroutine(SwitchScene());
                    }
                    else
                    {
                        canvas.GetComponentInChildren<Text>().text = "Pairing failed. Try to scan again"; 
                        print("Token Not Found");
                        scanning = false;
                    }

                },
                false);
            },
            TimeSpan.FromSeconds(10));
#endif

    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3);
        print("Trying to load LoadingScene");
        SceneManager.LoadScene("LoadingScene");

    }
    public void OnReset()
    {

    }
}
