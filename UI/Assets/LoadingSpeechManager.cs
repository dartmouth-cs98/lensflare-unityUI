using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.SceneManagement;
using Academy.HoloToolkit.Unity;

public class LoadingSpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    WorldAnchorStore store;

    void Start()
    {
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
        gameObject.GetComponent<SpatialMappingRenderer>().renderState = SpatialMappingRenderer.RenderState.None;

        keywords.Add("Create scene", () => {
            print("entering placement mode");
            if (store != null)
            {
                print("Clearing Store");
                store.Clear();
            }
            SceneManager.LoadScene("PlacementScene");
        });

        keywords.Add("Turn Off Mesh", () => {
            print("Turning Off Mesh");
            gameObject.GetComponent<SpatialMappingRenderer>().renderState = SpatialMappingRenderer.RenderState.None;
        });

        keywords.Add("Turn On Mesh", () => {
            print("Turning On Mesh");
            gameObject.GetComponent<SpatialMappingRenderer>().renderState = SpatialMappingRenderer.RenderState.Visualization;
        });

        keywords.Add("Pair Device", () => {
            print("Pair Device");
            SceneManager.LoadScene("PairingScene");
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

    private void AnchorStoreLoaded(WorldAnchorStore store)
    {
        this.store = store;
    }

    void Update()
    {

    }
}