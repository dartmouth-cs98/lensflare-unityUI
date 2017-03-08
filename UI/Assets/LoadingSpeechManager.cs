using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity;

public class LoadingSpeechManager : MonoBehaviour
{
    public GestureManagerUser gestureManager; 
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    WorldAnchorStore store;

    void Start()
    {
        WorldAnchorStore.GetAsync(AnchorStoreLoaded);
        //gameObject.GetComponent<SpatialMappingRenderer>().renderState = SpatialMappingRenderer.RenderState.None;

        keywords.Add("Lensflare create scene", () => {
            print("entering placement mode");
            ClearAllGems();

            SceneManager.LoadScene("PlacementScene");
        });

        //keywords.Add("Turn Off Mesh", () => {
        //    print("Turning Off Mesh");
        //    gameObject.GetComponent<SpatialMappingRenderer>().renderState = SpatialMappingRenderer.RenderState.None;
        //});

        //keywords.Add("Turn On Mesh", () => {
        //    print("Turning On Mesh");
        //    gameObject.GetComponent<SpatialMappingRenderer>().renderState = SpatialMappingRenderer.RenderState.Visualization;
        //});

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
        print("existing anchors loading:");

        for (int i = 0; i < store.GetAllIds().Length; i++)
        {
            print("id: " + store.GetAllIds()[i]);
        }
    }

    public void ClearAllGems()
    {
        if (store != null)
        {
            print("Clearing Store");
            store.Clear();
        }

        GameObject[] gems = GameObject.FindGameObjectsWithTag("GemCanvas");
        foreach (GameObject gem in gems)
        {
            GameObject.Destroy(gem);
        }

    }

    void Update()
    {

    }
}