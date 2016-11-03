﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

// Based on Holograms 101 tutorial 
public class SpeechManager : MonoBehaviour
{
    Photographer photographer;
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    bool testing = true;

    // Use this for initialization

    void Start()
    {
        photographer = GetComponent<Photographer>();

        keywords.Add("Translate", () => {
            print("Translating...");
            if (testing)
            {
                photographer.UseLocalPicture();
            } else {
                photographer.TakePicture();
            }
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
}