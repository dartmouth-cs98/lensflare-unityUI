﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Academy.HoloToolkit.Unity
{
    /// <summary>
    /// GestureManager creates a gesture recognizer and signs up for a tap gesture.
    /// When a tap gesture is detected, GestureManager uses GazeManager to find the game object.
    /// GestureManager then sends a message to that game object.
    /// </summary>
    /// 

    [RequireComponent(typeof(GazeManager))]
    public partial class GestureManager : Singleton<GestureManager>
    {
        /// <summary>
        /// To select even when a hologram is not being gazed at,
        /// set the override focused object.
        /// If its null, then the gazed at object will be selected.
        /// </summary>
        public GameObject OverrideFocusedObject
        {
            get; set;
        }

        /// <summary>
        /// Gets the currently focused object, or null if none.
        /// </summary>
        public GameObject FocusedObject
        {
            get { return focusedObject; }
        }

        private GestureRecognizer gestureRecognizer;
        private GameObject focusedObject;

        private bool dragging = false;
        private GameObject draggedGO;
        private float iconOffset;


        void Start()
        {
            iconOffset = GameObject.Find("GemCanvasPrefab").GetComponentInChildren<Collider>().bounds.size.magnitude / 4;

            // Create a new GestureRecognizer. Sign up for tapped events.
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
            gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;

            // Start looking for gestures.
            gestureRecognizer.StartCapturingGestures();
        }

        private void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            if (focusedObject != null)
            {
                IconManager iconManager = gameObject.GetComponent<IconManager>();

                if (dragging)
                {
                    print("done dragging");
                    dragging = false;
                    //draggedGO.layer = 0;
                    //draggedGO.transform.GetChild(0).gameObject.layer = 0;
                    draggedGO.transform.FindChild("GemWrapper").transform.FindChild("Gem").gameObject.layer = 0;
                    iconManager.SaveAnchor(draggedGO);
                }
                else
                { 
                    print(focusedObject);

                    if (focusedObject.tag == "Gem")
                    {
                        //GemBehavior gb = focusedObject.transform.parent.GetComponent<GemBehavior>();
                        //gb.Select();
                        print("Setting dragging" + focusedObject);
                        dragging = true;
                        draggedGO = focusedObject.transform.parent.transform.parent.transform.gameObject;
                        //draggedGO.layer = 2;
                        //draggedGO.transform.GetChild(0).gameObject.layer = 2;
                        focusedObject.gameObject.layer = 2;
                        iconManager.DeleteAnchor(draggedGO);
                    }
                    else
                    {
                        RaycastHit hit = GazeManager.Instance.HitInfo;
                        Vector3 vect = hit.point + (hit.normal * iconOffset);
                        gameObject.GetComponent<IconManager>().PlaceBox(vect);
                    }
                }
            } 
        }

        void LateUpdate()
        {
            GameObject oldFocusedObject = focusedObject;

           

            if (GazeManager.Instance.Hit &&
                OverrideFocusedObject == null &&
                GazeManager.Instance.HitInfo.collider != null)
            {
                // If gaze hits a hologram, set the focused object to that game object.
                // Also if the caller has not decided to override the focused object.
                focusedObject = GazeManager.Instance.HitInfo.collider.gameObject;
            }
            else
            {
                // If our gaze doesn't hit a hologram, set the focused object to null or override focused object.
                focusedObject = OverrideFocusedObject;
            }

            if (focusedObject != oldFocusedObject)
            {
                // If the currently focused object doesn't match the old focused object, cancel the current gesture.
                // Start looking for new gestures.  This is to prevent applying gestures from one hologram to another.
                gestureRecognizer.CancelGestures();
                gestureRecognizer.StartCapturingGestures();
            }

            if (dragging)
            {
                print("Moving gem");

                RaycastHit hit = GazeManager.Instance.HitInfo;
                Vector3 vect = hit.point + (hit.normal * iconOffset);
                //Vector3 vect = hit.point;
                draggedGO.transform.position = vect;
            }
        }

        void OnDestroy()
        {
            gestureRecognizer.StopCapturingGestures();
            gestureRecognizer.TappedEvent -= GestureRecognizer_TappedEvent;
        }
    }
}