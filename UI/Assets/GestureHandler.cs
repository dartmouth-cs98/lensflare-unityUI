using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using Academy.HoloToolkit.Unity;

public class GestureHandler : Singleton<GestureHandler>
{
    CreateBox cb;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnAirTapped()
    {
        Vector3 vect = new Vector3(GazeManager.Instance.HitInfo.point.x, GazeManager.Instance.HitInfo.point.y, GazeManager.Instance.HitInfo.point.z);
      //  cb.PlaceBox(vect);
    }
}
