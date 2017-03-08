using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCamera : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Vector3 newPos = Camera.main.transform.position;
        newPos.z = Camera.main.farClipPlane;
        gameObject.transform.position = newPos;
    }

}
