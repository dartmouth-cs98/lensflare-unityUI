using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;
using System.Net;
using System.Collections.Generic;

// Based on Holograms 101 tutorial 
public class SpeechManager : MonoBehaviour
{
    public Rotate rotate;
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
        //uploadImages = GetComponent<UploadImages>();
        photographer = GetComponent<Photographer>();
        tsm = GetComponent<TextToSpeechManager>();

        keywords.Add("Next", () => {
            print("Closing canvas...");

            //setupAnim.ShrinkCanvas();
            doneAnim.GrowCanvas();

        });

        keywords.Add("Done", () => {
            print("Closing canvas...");
            
            doneSetup();
            //call photo upload method

        });


        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        //                          OLD WORK                                  //
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////
        keywords.Add("Translate", () => {
            print("Translating...");

            tsm.SpeakText("Translating to English");

            photographer.SetMode("translate");
            photographer.SetLanguage("en");
            rotate.indicator_visible = true;
            if (testing)
            {
                photographer.UseLocalPicture();
            }
            else
            {
                photographer.TakePicture(GetTranslationFilepath(), false);
            }
        });

        keywords.Add("Translate to Spanish", () => {
            print("Translating...");

            tsm.SpeakText("Translating to Spanish");

            photographer.SetMode("translate");
            photographer.SetLanguage("es");
            rotate.indicator_visible = true;
            if (testing)
            {
                photographer.UseLocalPicture();
            }
            else
            {
                photographer.TakePicture(GetTranslationFilepath(), false);
            }
        });
  
        keywords.Add("Lensflare, turn off audio", () => {
            print("Turning off audio...");

            tsm.SpeakText("Turning audio off");
            audioOn = false;
            photographer.SetAudio(false);

        });

        keywords.Add("Lensflare, turn on audio", () => {
            print("Turning on audio...");

            tsm.SpeakText("Turning audio on");
            audioOn = true;
            photographer.SetAudio(true);

        });
        
        keywords.Add("Lensflare, turn off visuals", () => {
            print("Turning off visuals...");

            if (audioOn) tsm.SpeakText("Turning visuals off");
            visualsOn = false;
            photographer.SetVisuals(false);

        });

        keywords.Add("Lensflare, turn on visuals", () => {
            print("Turning on visuals...");

            if (audioOn) tsm.SpeakText("Turning visuals on");
            visualsOn = true;
            photographer.SetVisuals(true);

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

        GetComponent<UploadImages>().StartUploadImages(localPaths, s3Paths, "dog@food.com", "test");
        doneAnim.ShrinkCanvas();

    }

    public void doneSetup()
    {
        print("Done Flow start");
        uploadingAnim.GrowCanvas();
        doneAnim.ShrinkCanvas();
        PerformImageUpload();
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