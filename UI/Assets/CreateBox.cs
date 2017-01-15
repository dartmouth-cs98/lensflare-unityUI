using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

namespace Academy.HoloToolkit.Unity
{
    public class CreateBox : MonoBehaviour
    {
        GameObject wireframe;
        WorldAnchorStore store;
        List<GameObject> anchors;

        // Use this for initialization
        void Start()
        {
            WorldAnchorStore.GetAsync(AnchorStoreLoaded);

            anchors = new List<GameObject>();

            wireframe = Instantiate(Resources.Load("IconPrefabWireframe")) as GameObject;
            wireframe.layer = 2;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vect = new Vector3(GazeManager.Instance.HitInfo.point.x, GazeManager.Instance.HitInfo.point.y, GazeManager.Instance.HitInfo.point.z);
            wireframe.transform.position = vect;

            if (Input.GetButtonDown("x360_A"))
            {
                GameObject icon = Instantiate(Resources.Load("IconPrefabWire")) as GameObject;
                icon.transform.position = vect;
            }
        }

        private void AnchorStoreLoaded(WorldAnchorStore store)
        {
            this.store = store;

            string[] anchorIds = this.store.GetAllIds();
            for (int i = 0; i < anchorIds.Length; i++)
            {
                
            }


        }
    }

}
