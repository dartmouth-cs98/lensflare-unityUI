using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Academy.HoloToolkit.Unity
{
    public class CreateBox : MonoBehaviour
    {
        GameObject wireframe;
        //WorldAnchorStore store;
        List<GameObject> anchors;

        // Use this for initialization
        void Start()
        {
            //WorldAnchorStore.GetAsync(AnchorStoreLoaded);

            anchors = new List<GameObject>();

            wireframe = Instantiate(Resources.Load("GemPrefab/Prefab/GemParticleWorking")) as GameObject;
            wireframe.layer = 2;
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit = GazeManager.Instance.HitInfo;

            // Adjust magnitude of size something
            //Vector3 vect = hit.point + (hit.normal * wireframe.GetComponent<Collider>().bounds.size.magnitude / 2);
           // wireframe.transform.position = vect;
        }
    }
}
