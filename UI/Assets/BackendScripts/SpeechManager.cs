using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;
using System.Net;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Input;
using Academy.HoloToolkit.Unity;
using UnityEngine.VR.WSA;



// Based on Holograms 101 tutorial 
public class SpeechManager : MonoBehaviour
{
    public Rotate rotate;
    //public UnityEngine.VR.WSA.SpatialMappingRenderer spatialMappingRenderer;
    public GestureManager gestureManager; 

    //public TextToSpeechManager tsm;
    TextToSpeechManager tsm;

    Photographer photographer;
    //CanvasAnimation anim;
    CanvasAnimation uploadingAnim;
    CanvasAnimation doneAnim;
    UploadImages uploadImages;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    bool testing = false;
    bool audioOn = true;
    bool visualsOn = true;

    // Use this for initialization

    void Start()
    {
        uploadingAnim = GameObject.FindGameObjectWithTag("UploadFlow").GetComponent<CanvasAnimation>();
        doneAnim = GameObject.FindGameObjectWithTag("DoneCanvas").GetComponent<CanvasAnimation>();
        print("done anim" + doneAnim.ToString());
        print("Grow canvas");
        doneAnim.GrowCanvas();
        //uploadImages = GetComponent<UploadImages>();
        photographer = GetComponent<Photographer>();

        tsm = GetComponent<TextToSpeechManager>();

        string deviceToken = PlayerPrefs.GetString("device_token", "");
        Debug.Log(deviceToken);
        if (deviceToken.Equals(""))
        {
            SceneManager.LoadScene("PairingScene");
            return;
        }

        //spatialMappingRenderer.renderState = SpatialMappingRenderer.RenderState.None;

        //keywords.Add("Next", () => {
        //    print("Closing canvas...");

        //    //setupAnim.ShrinkCanvas();
        //    doneAnim.GrowCanvas();

        //});

        keywords.Add("Done", () => {
            print("Closing canvas...");
            
            doneSetup();
            //calls photo upload, export anchors methods

        });

        //keywords.Add("Turn Off Mesh", () => {
        //    print("Turning Off Mesh");
        //   spatialMappingRenderer.renderState = SpatialMappingRenderer.RenderState.None;
        //});

        //keywords.Add("Turn On Mesh", () => {
        //    print("Turning On Mesh");
        //    spatialMappingRenderer.renderState = SpatialMappingRenderer.RenderState.Visualization;
        //});

       
        
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

    private string GetTranslationFilepath()
    {
        return "translationImage" + DateTime.Now.ToString("yyddMHHmmss");
    }

    private string[] GetIconIds()
    {
        IconManager im = GameObject.Find("Cursor_box").GetComponent<IconManager>();
        return im.GetAnchorStore().GetAllIds();
    }

    public void PerformImageUpload()
    {
        string[] ids = GetIconIds();
        string[] localPaths = new string[ids.Length];
        string[] s3Paths = new string[ids.Length];

        for (int i = 0; i < ids.Length; i++)
        {
            localPaths[i] = System.IO.Path.Combine(Application.persistentDataPath, ids[i] + ".jpg");
            s3Paths[i] = ids[i] + ".jpg";
        }
        //localPaths[0] = "aaa.jpg";

        string deviceToken = PlayerPrefs.GetString("device_token", "");

        Debug.Log("this is happening right now");
        Debug.Log("device token: " + deviceToken);
        GetComponent<UploadImages>().StartUploadFiles(localPaths, s3Paths, deviceToken, (url) =>
        {
            return true;
        });
    }

    public void doneSetup()
    {
        print("Done Flow start");
        uploadingAnim.GrowCanvas();
        doneAnim.ShrinkCanvas();
        PerformImageUpload();
        GameObject.Find("Cursor_box").GetComponent<IconManager>().MakeTransferBatch();
    }

    void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            doneSetup();
        }

        if (Input.GetKeyDown("b"))
        {
            //setupAnim.ChangeSprite("get_started");
        }

        if (Input.GetKeyDown("n"))
        {
            //setupAnim.GrowCanvas();
        }
    }
}