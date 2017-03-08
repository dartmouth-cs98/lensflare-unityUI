using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity;

public class VoiceAssistant : MonoBehaviour {

	// Use this for initialization
	void Start () {
        TextToSpeechManager textToSpeech = GetComponent<TextToSpeechManager>();
        textToSpeech.Voice = TextToSpeechVoice.Zira;

        // Get the active scene
        Scene scene = SceneManager.GetActiveScene();
        if (string.Equals(scene.name, "StartupScene", System.StringComparison.Ordinal))
        {
            textToSpeech.SpeakText("Welcome to Lensflare!");
        }
        if (string.Equals(scene.name, "LoadingScene", System.StringComparison.Ordinal))
        {
            textToSpeech.SpeakText("Select a gem to see media!");
        }
        if (string.Equals(scene.name, "PairingScene", System.StringComparison.Ordinal))
        {
            textToSpeech.SpeakText("Scan the QR code to sync your HoloLens!");
        }
        if (string.Equals(scene.name, "PlacementScene", System.StringComparison.Ordinal))
        {
            textToSpeech.SpeakText("Place gems to create your own space!");
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
