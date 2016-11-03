using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.WebCam;
using System.Linq;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class Photographer : MonoBehaviour
{
    public SetDialogueText sdt;

    string TIMEFORMAT = "yyddMHHmmss";
    string TESTFILE = "Brooklyn_Bridge_Postdlf.jpg";

    System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    PhotoCapture photo = null;
    string filepath = "";
    string detections = "";
    string translations = "";
    ArrayList annotations = new ArrayList();

    public void Start()
    {
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

    // IN PROGRESS
    // need to translate everything in the ArrayList textDetections
    public void TranslateText(string targetLang)
    {
        // Google Translate API static values
        string apiKey = "AIzaSyA8Gc4eafOjnhnj5QYn7I2ZNEumpxtxY-s";
        string translateURL = "https://www.googleapis.com/language/translate/v2?key=" + apiKey;

        string query = "&q=";
        string source = "&source=";
        string target = "&target=" + targetLang;

        ArrayList textDetections = GoogleVisionParser.parseTextAnnotations(detections);
        source = source + ((TextAnnotation) textDetections[0]).getLocale(); // source language

        // Google Translate returns an error when source and target language are the same
        // currently simply notifying the user if this case arises
        if (source.Equals(targetLang, StringComparison.Ordinal))
        {
            for (int i = 0; i < textDetections.Count; i++)
            {
                translations = translations + ' ' + ((TextAnnotation)textDetections[i]).getDescription();
            }
            translations = translations.Trim();
            Debug.Log(translations);
            Debug.Log("Warning: same source and target language.");
        }
        else
        {
            query = ((TextAnnotation)textDetections[0]).getDescription();
            for (int i =  1; i < textDetections.Count; i++)
            {
                query = query + '+' + ((TextAnnotation)textDetections[i]).getDescription();
            }
            
            translateURL = translateURL + query + source + target;
            Debug.Log(translateURL);

            WWW www = new WWW(translateURL);
            StartCoroutine(WaitForRequest(www, "Translate"));
        }
    }

    // READY FOR TESTING
    public void SearchWiki()
    {
        // Wiki Search API static values
        string searchURL = "https://en.wikipedia.org/w/api.php?action=opensearch&limit=1&format=json&search=";

        // TODO: iterate through and process the entire list of landmark annotations
        ArrayList landmarks = (ArrayList)GoogleVisionParser.parseAllAnnotations(detections)[2];
        if (landmarks.Count > 0)
        {
            string search = ((LandmarkAnnotation)landmarks[0]).getDescription().Replace(" ", "%20");
            searchURL = searchURL + search.Substring(1, search.Length - 1);
            Debug.Log(searchURL);

            WWW www = new WWW(searchURL);
            StartCoroutine(WaitForRequest(www, "WikiSearch"));
        }
        else
        {
            Debug.Log("No landmarks detected.");
        }
    }

    // READY FOR TESTING
    public void QueryWiki(string title)
    {
        // Wiki Search API static values
        string queryURL = "https://en.wikipedia.org/w/api.php?action=query&prop=extracts&exintro=&explaintext=&format=json&redirects=&titles=";

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
                detections = www.text;
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

                SearchWiki();
            }
            else if (mode.Equals("Translate", StringComparison.Ordinal))
            {
                translations = www.text;
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
                    string subqueryURL = www.url.Substring(www.url.LastIndexOf("%20"));
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
                /* CLEAN THIS UP OH MY GOD EW */
                Debug.Log(www.text);
                string pageTitle = www.text.Substring(www.text.IndexOf("\"title\":")+8, www.text.IndexOf("\"extract\":")-www.text.IndexOf("\"title\":")-9);
                string pageExtract = www.text.Substring(www.text.IndexOf("\"extract\":")+10, 300);
                pageTitle = pageTitle.Replace("\n", " ");
                pageExtract = pageExtract.Replace("\n", " ");
                pageTitle = pageTitle.Replace("\\", "");
                pageExtract = pageExtract.Replace("\\", "");
                pageTitle = pageTitle.Replace("\"", "");
                pageExtract = pageExtract.Replace("\"", "");
                pageExtract = pageExtract.Replace("}", "");
                sdt.UpdateText(pageTitle, pageExtract);
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}

