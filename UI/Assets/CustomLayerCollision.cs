using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLayerCollision : MonoBehaviour
{
    // Update is called once per frame.
    void Update()
    {
        // When the user presses the left mouse button,
        // Do a collision test.  You could fire the
        // DetectCollisions based on a gesture event.*

        if (Input.GetMouseButtonDown(0))
        {
            DetectCollisions();
        }
    }

    void DetectCollisions()
    {
        // Raycast against all game objects that are on either the
        // spatial surface or UI layers.
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        // We use ScreenPointToRay to create a ray whose origin is the
        // main camera's position and direction is from the position of the main
        // camera to the position of where the mouse position would be in world space.
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), float.MaxValue, layerMask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(string.Format("Hit Object **\"**{0}**\"** at position **\"**{1}**\"**", hit.collider.gameObject, hit.point));
            }
        }
        else
        {
            Debug.Log("Nothing was hit.");
        }
    }
}