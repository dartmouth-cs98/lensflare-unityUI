//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.VR.WSA.Persistence;
//using UnityEngine.VR.WSA;
//using UnityEngine.Windows.Speech;
//using System.Linq;


//public class AnchorObject : MonoBehaviour
//{

//    WorldAnchorStore store;
//    public string iconName;
//    public bool savedRoot = false;

//    KeywordRecognizer keywordRecognizer = null;
//    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

//    // Use this for initialization
//    void Start()
//    {
//        print("Start POS:" + gameObject.transform.position);
//        print("icon name is " + iconName);
//        WorldAnchorStore.GetAsync(AnchorStoreLoaded);

//        // Testing purposes
//        keywords.Add("Move", () =>
//        {
//            print("moved");
//            MoveAnchor(new Vector3(0.2f, 0.5f, 1.2f));
//        });

//        keywords.Add("Clear", () =>
//        {
//            print("cleared");
//            this.store.Clear();
//        });

//        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

//        // Register a callback for the KeywordRecognizer and start recognizing!
//        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
//        keywordRecognizer.Start();
//    }

//    // Testing
//    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
//    {
//        System.Action keywordAction;
//        if (keywords.TryGetValue(args.text, out keywordAction))
//        {
//            keywordAction.Invoke();
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown("f"))
//        {
//            print("F pressed");
//            MoveAnchor(new Vector3(0.2f, 0.5f, 1.2f));
//            print("moved to:" + gameObject.transform.position.ToString());
//        }
//        if (Input.GetKeyDown("g"))
//        {
//            print("G pressed");
//            MoveAnchor(new Vector3(0.4f, 0.5f, 1.2f));
//            print("moved to:" + gameObject.transform.position.ToString());
//        }

//        if (Input.GetKeyDown("c"))
//        {
//            print("cleared");
//            this.store.Clear();
//        }

//    }

//    private void SaveAnchor()
//    {
//        //if (!this.savedRoot)
//        {
//            print("Saving root");

//            WorldAnchor anchor = gameObject.GetComponent<WorldAnchor>();
//            print("anchor label: " + anchor);
//            if (anchor == null)
//            {
//                anchor = gameObject.AddComponent<WorldAnchor>();

//                //if (anchor.isLocated)
//                //{
//                    print("saving :" + iconName);
//                    this.store.Save(iconName, anchor);
//                //}
//                //else
//                //{
//                //    anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
//                //}
//            }

//        }
//    }

//    private void MoveAnchor(Vector3 vect)
//    {

//        if (gameObject.GetComponent<WorldAnchor>() == null)
//        {
//            gameObject.transform.position = vect;
//            SaveAnchor();
//        }
//        else
//        {
//            this.store.Delete(iconName);
//            DestroyImmediate(gameObject.GetComponent<WorldAnchor>());

//            gameObject.transform.position = vect;
//            WorldAnchor anchor = gameObject.AddComponent<WorldAnchor>();
//            if (anchor.isLocated)
//            {
//                print("saving :" + iconName);
//                this.savedRoot = this.store.Save(iconName, anchor);
//            }
//            else
//            {
//                anchor.OnTrackingChanged += Anchor_OnTrackingChanged;
//            }
//        }
//    }

//    private void AnchorStoreLoaded(WorldAnchorStore store)
//    {
//        this.store = store;

//        string[] anchorIds = this.store.GetAllIds();
//        print(anchorIds.Length);
//        for (int i = 0; i < anchorIds.Length; i++)
//        {
//            print("anchor #:" + anchorIds[i]);
//            if (anchorIds[i] == iconName)
//            {
//                print("Found " + iconName);
//                WorldAnchor anchor = this.store.Load(anchorIds[i], gameObject);
//                print("loaded anchor position: " + anchor.transform.position);
//                this.savedRoot = true;
//                break;
//            }
//        }

//        SaveAnchor();
//    }

//    private void Anchor_OnTrackingChanged(WorldAnchor anchor, bool located)
//    {
//        anchor.gameObject.SetActive(located);
//    }
//}
