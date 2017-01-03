using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.WebCam;
using System.Linq;
using System;
using System.Net;
using System.Collections.Generic;
using Assets.Scripts;
using HoloToolkit.Unity;
using UnityEngine.Windows.Speech;
//using System.Net;


public class Photographer : MonoBehaviour
{
    public SetDialogueText sdt;
    public Rotate rotate;
    TextToSpeechManager ttsm; 

    string TIMEFORMAT = "yyddMHHmmss";
    string TESTFILE = "sign.jpg";

    System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    PhotoCapture photo = null;
    string filepath = "";
    ArrayList annotations = new ArrayList();
    int maxLength = 235;
    bool audioOn = true;
    bool visualsOn = true;
    string currAction = "detect";
    string language = "en";

    public void SetAudio(bool audio)
    {
        this.audioOn = audio;
    }

    public void SetVisuals(bool visuals)
    {
        this.visualsOn = visuals;
    }

    public void SetMode(string mode)
    {
        this.currAction = mode;
    }

    public void SetLanguage(string language)
    {
        this.language = language;
    }

    public void Start()
    {
        ttsm = GetComponent<TextToSpeechManager>();
        print("photographer started");
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }

    public void TakePicture()
    {
        while (photo == null)
        {
            print("waiting to enter photo mode");
        }

        print("taking pic..");
        timer.Start();
        photo.TakePhotoAsync(filepath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
    }

    public void UseLocalPicture() {
        byte[] imgBytes = System.IO.File.ReadAllBytes(TESTFILE);
        string imgBase64 = Convert.ToBase64String(imgBytes);

        Annotate(imgBase64);
    }

    // Set up camera parameters, enter photo mode
    void OnPhotoCaptureCreated(PhotoCapture capture)
    {
        print("capture created");
        photo = capture;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters cameraParams = new CameraParameters();
        cameraParams.hologramOpacity = 0.0f;
        cameraParams.cameraResolutionWidth = cameraResolution.width;
        cameraParams.cameraResolutionHeight = cameraResolution.height;
        cameraParams.pixelFormat = CapturePixelFormat.BGRA32;

        capture.StartPhotoModeAsync(cameraParams, false, OnPhotoModeStarted);
    }

    // Enter photo mode, set path
    void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            print("photo mode started");
            filepath = System.IO.Path.Combine(Application.persistentDataPath, "translationImage" + DateTime.Now.ToString(TIMEFORMAT) + ".jpg");
        }
        else
        {
            Debug.LogError("Error starting photo mode.");
        }
    }

    // Cleanup
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photo.Dispose();
        photo = null;
    }

   
    // Cleanup on completion
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            byte[] imgBytes = System.IO.File.ReadAllBytes(filepath);
            string imgBase64 = Convert.ToBase64String(imgBytes);

            // GET
            print("captured photo to disk: " + filepath);
            print("time to save pic:" + timer.ElapsedMilliseconds);

            Annotate(imgBase64);
        }
        else
        {
            Debug.Log("ERROR: " + result.ToString());
        }

        //TODO - delete photos after translation completes (if we decide to accumulate pics instead of overwriting)
    }

    public void Annotate(string imgBase64)
    {
        // Google Vision API static values
        string apiKey = "AIzaSyA8Gc4eafOjnhnj5QYn7I2ZNEumpxtxY-s";
        string visionURL = "https://vision.googleapis.com/v1/images:annotate?key=" + apiKey;

        // TODO: write a distinct function for sending *customizable* JSON requests
        // currently hard-coded to include detection of: LABEL, TEXT, FACE, LANDMARK, LOGO
        // currently hard-coded to return maximum results for each detection of: 10 
        string jsonRequest = @"{ 
            ""requests"":[ 
                { 
                    ""image"":{ 
                        ""content"":" + "\"" + imgBase64 + "\"" +
                    @"}, 
                    ""features"":[ 
                        { 
                            ""type"":""LABEL_DETECTION"", 
                            ""maxResults"":10 
                        },
                        { 
                            ""type"":""TEXT_DETECTION"", 
                            ""maxResults"":10 
                        }, 
                        { 
                            ""type"":""FACE_DETECTION"", 
                            ""maxResults"":10 
                        }, 
                        { 
                            ""type"":""LANDMARK_DETECTION"", 
                            ""maxResults"":10 
                        }, 
                        { 
                            ""type"":""LOGO_DETECTION"", 
                            ""maxResults"":10 
                        }
                    ] 
                } 
            ] 
        }";

        var encoding = new System.Text.UTF8Encoding();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        WWW www = new WWW(visionURL, encoding.GetBytes(jsonRequest), headers);
        StartCoroutine(WaitForRequest(www, "Detect"));
    }

    // READY FOR TESTING
    // need to translate everything in the ArrayList textDetections
    public void TranslateText(ArrayList textDetections, string targetLang)
    {
        // Google Translate API static values
        string apiKey = "AIzaSyA8Gc4eafOjnhnj5QYn7I2ZNEumpxtxY-s";
        string translateURL = "https://www.googleapis.com/language/translate/v2?key=" + apiKey;

        string query = "&q=";
        string source = "&source=";
        string target = "&target=" + targetLang;

        ArrayList textDetections = (ArrayList) annotations[0];
        if (textDetections.Count > 0)
        {
            string sourceLang = ((TextAnnotation)textDetections[0]).getLocale();
            sourceLang = sourceLang.Replace("\"", "");

            // Google Translate returns an error when source and target language are the same
            // currently simply notifying the user if this case arises
            if (sourceLang.Equals(targetLang, StringComparison.Ordinal))
            {
                string translations = "";
                for (int i = 0; i < textDetections.Count; i++)
                {
                    translations = translations + ' ' + ((TextAnnotation)textDetections[i]).getDescription();
                }
                translations = translations.Trim();

                if (visualsOn) sdt.UpdateText("Translation to " + language, translations);
                Debug.Log(translations);

                Debug.Log(translations);
                Debug.Log("Warning: same source and target language.");
            }
            else
            {
                source = source + sourceLang;
                //query = ((TextAnnotation)textDetections[0]).getDescription();
                for (int i = 1; i < textDetections.Count; i++)
                {
                    //https://www.googleapis.com/language/translate/v2?key=AIzaSyA8Gc4eafOjnhnj5QYn7I2ZNEumpxtxY-s&q=THE"%20END"%20IS"%20NEAR"&source="en"&target=en

                    string td = ((TextAnnotation)textDetections[i]).getDescription();
                    td = td.Replace("\"", "");
                    if (i > 1) query = query + "%20" + td;
                    else query = query + td;
                }

                translateURL = translateURL + query + source + target;
                Debug.Log(translateURL);

                WWW www = new WWW(translateURL);
                StartCoroutine(WaitForRequest(www, "Translate"));
            }
        }
        else
        {
            Debug.Log("No text detected.");
        }
        

        
    }

    // for searching Wikipedia for matching page titles
    public void SearchWiki(ArrayList landmarks)
    {
        // Wiki Search API static values
        string searchURL = "https://en.wikipedia.org/w/api.php?action=opensearch&limit=1&format=json&search=";
        // TODO: iterate through and process the entire list of landmark annotations
        ArrayList landmarks = (ArrayList)annotations[2];
        ArrayList labels = (ArrayList)annotations[1];
        if (landmarks.Count > 0)
        {
            string search = ((LandmarkAnnotation) landmarks[0]).getDescription().Replace(" ", "%20");
            searchURL = searchURL + search.Substring(1, search.Length - 1);
            Debug.Log(searchURL);

            WWW www = new WWW(searchURL);
            StartCoroutine(WaitForRequest(www, "WikiSearch"));
        }
        else
        {
            string speechString = "I couldn't find any landmarks, but it seems that you're looking at the following. ";
            string textString = "I couldn't find any landmarks, but it seems that you're looking at the following: ";
            for (int i = 0; i < labels.Count; i++)
            {
                speechString += ((LabelAnnotation)labels[i]).getDescription() + ", ";
                if (i != labels.Count - 1) textString += ((LabelAnnotation)labels[i]).getDescription() + ", ";
                else textString += ((LabelAnnotation)labels[i]).getDescription() + ".";
            }
            if (audioOn) ttsm.SpeakText(speechString);
            if (visualsOn) sdt.UpdateText("No landmarks found", textString);
        }
    }

    // for querying for Wikipedia page results given a proper page title
    public void QueryWiki(string title)
    {
        // Wiki Search API static values
        string queryURL = "https://en.wikipedia.org/w/api.php?action=query&prop=extracts|pageimages&exintro=&explaintext=&format=json&redirects=&pithumbsize=300&titles=";

        queryURL = queryURL + title.Replace(" ", "%20");
        Debug.Log(queryURL);

        WWW www = new WWW(queryURL);
        StartCoroutine(WaitForRequest(www, "WikiQuery"));
    }

    IEnumerator WaitForRequest(WWW www, string mode)
    {
        yield return www;

        if (www.error == null)
        {
            if (mode.Equals("Detect", StringComparison.Ordinal))
            {
                string detections = www.text;
                Debug.Log(detections);

                annotations = GoogleVisionParser.parseAllAnnotations(detections);
                
                for (int i = 0; i < annotations.Count; i++)
                {
                    for (int j = 0; j < ((ArrayList) annotations[i]).Count; j++)
                    {
                        Debug.Log("\n" + ((ArrayList)annotations[i])[j]);
                    }
                }

                print("time to get response:" + timer.ElapsedMilliseconds);
                timer.Stop();

                if (currAction.Equals("translate")) TranslateText(language);
                if (currAction.Equals("detect")) SearchWiki();
            }
            else if (mode.Equals("Translate", StringComparison.Ordinal))
            {
                string translations = www.text;
                Debug.Log(translations);

                int start = www.text.IndexOf("\"translatedText\":") + 17;
                translations = translations.Substring(start);

                int finish = translations.IndexOf("}");
                translations = translations.Substring(0, finish);

                string[] repChars = {"\"", "{", "}", "[", "]" };
                for (int i = 0; i < repChars.Length; i++)
                {
                    translations = translations.Replace(repChars[i], "");
                }

                if (visualsOn) sdt.UpdateText("Translation to " + language.ToUpper(), translations);
                Debug.Log(translations);
            }
            else if (mode.Equals("WikiSearch", StringComparison.Ordinal))
            {
                Debug.Log(www.text);

                // extract the page found, if any
                int start = www.text.IndexOf('[', www.text.IndexOf('[') + 1) + 2;
                int length = www.text.IndexOf(']') - start - 1;
                string title = www.text.Substring(start, length);

                if (!String.IsNullOrEmpty(title)) // get the extract from the page found
                {
                    QueryWiki(title); 
                }
                else // continue searching through substrings for potential pages of interest
                {
                    // currently takes away last word in query for each subquery (could improve on this heuristic)
                    string subqueryURL = www.url.Substring(0,www.url.LastIndexOf("%20"));
                    Debug.Log(subqueryURL);
                    if (!String.IsNullOrEmpty(subqueryURL))
                    {
                        WWW recWWW = new WWW(subqueryURL);
                        StartCoroutine(WaitForRequest(www, "WikiSearch"));
                    }
                    else
                    {
                        Debug.Log(www.url);
                        Debug.Log("Error: no page found.");
                    }
                }
            }
            else // mode == "WikiQuery"
            {
                string[] repChars = {"\\n", "\\", "\"", "}" };

                Debug.Log(www.text);

                // will this always work???
                // should include checks on this
                string pageTitle = www.text.Substring(www.text.IndexOf("\"title\":")+8, www.text.IndexOf("\"extract\":")-www.text.IndexOf("\"title\":")-9);
                string pageExtract = www.text.Substring(www.text.IndexOf("\"extract\":")+10);

                // TEST THIS
                //string imgSrc = www.text.Substring(www.text.IndexOf("\"source\":") + 9, www.text.IndexOf("\"width"\":") - www.text.IndexOf("\"source\":") - 10);
                //imgSrc = imgSrc.Substring(1, imgSrc.Length - 1);

                if (pageExtract.Length > maxLength)
                {
                    pageExtract.Substring(0, maxLength);
                }

                for (int i = 0; i < repChars.Length; i++)
                {
                    pageTitle = pageTitle.Replace(repChars[i], "");
                    pageExtract = pageExtract.Replace(repChars[i], "");
                }

                // TEST THIS
                //if (!String.IsNullOrEmpty(imgSrc))
                //{
                //    using (WebClient client = new WebClient())
                //    {
                //        client.DownloadFileAsync(new Uri(imgSrc), @"./Assets/Images/" + pageTitle + ".png");
                //    }
                //}
                
                Debug.Log("ABOUT TO UPDATE TEXT");
                Debug.Log(pageTitle);
                Debug.Log(pageExtract);
                if (audioOn) ttsm.SpeakText(pageTitle);
                if (visualsOn) sdt.UpdateText(pageTitle, pageExtract);
            }
        }
        else
        {
            Debug.Log(www.error);
        }
        rotate.indicator_visible = false;
    }
}

