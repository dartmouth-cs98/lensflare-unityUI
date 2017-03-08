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
    Animator uploadingAnim;
    Animator doneAnim;
    Animator instructionAnim; 
    UploadImages uploadImages;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    bool testing = false;
    bool audioOn = true;
    bool visualsOn = true;

    // Use this for initialization
    void Start()
    {
        uploadingAnim = GameObject.FindGameObjectWithTag("UploadFlow").GetComponent<Animator>();
        doneAnim = GameObject.FindGameObjectWithTag("DoneCanvas").GetComponent<Animator>();
        instructionAnim = GameObject.FindGameObjectWithTag("InstructionCanvas").GetComponent<Animator>();

        print("done anim" + doneAnim.ToString());
        print("Grow canvas");
        instructionAnim.SetTrigger("StateChange");
        photographer = GetComponent<Photographer>();

        tsm = GetComponent<TextToSpeechManager>();

        string deviceToken = PlayerPrefs.GetString("device_token", "");
        Debug.Log(deviceToken);

        keywords.Add("Done", () =>
        { 
            if (GameObject.Find("Cursor_box").GetComponent<IconManager>().GetAnchorStore().GetAllIds().Length == 0)
            {
                print("No gems in scene");
                GameObject.FindGameObjectWithTag("DoneCanvas").GetComponent<ChangeMaterial>().Change("done_error");
            }
            else
            {
                print("Closing canvas...");
                doneSetup();
            }
        });

        keywords.Add("Delete", () =>
        {
            print("Deleting gem...");
            gestureManager.DeleteGem();
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
        doneAnim.SetTrigger("StateChange");
        uploadingAnim.SetTrigger("StateChange");
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