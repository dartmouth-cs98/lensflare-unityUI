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

            wireframe = Instantiate(Resources.Load("IconPrefabWireframe")) as GameObject;
            wireframe.layer = 2;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 vect = new Vector3(GazeManager.Instance.HitInfo.point.x, GazeManager.Instance.HitInfo.point.y, GazeManager.Instance.HitInfo.point.z);
            wireframe.transform.position = vect;
        }

        //public void PlaceBox(Vector3 vect)
        //{
        //    GameObject icon = Instantiate(Resources.Load("IconPrefab")) as GameObject;
        //    AnchorObject ao = icon.GetComponent<AnchorObject>();
        //    ao.iconName = "icon-" + System.DateTime.Now.ToString("MM-dd-yy-H-mm-ss-fff");
        //    print("instantiating " + ao.iconName);
        //    ao.transform.position = vect;
        //}
    }
}
