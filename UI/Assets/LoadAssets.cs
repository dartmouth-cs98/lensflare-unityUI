using System.Collections;
using UnityEngine;

public class LoadAssets : MonoBehaviour {

	public string COLLIDER_TAG;
    public GameObject modelWrapper;
	public GemPrefabAnimator gpa;

    string previousUrl = null; 

    public void DownloadModel(string url)
    {
        if (url != previousUrl)
        {
            StartCoroutine(Load(url));
            previousUrl = url;
        }
    }

	public IEnumerator Load(string url) {
		WWW www = WWW.LoadFromCacheOrDownload (url ,1);
		yield return www;

		foreach (string an in www.assetBundle.GetAllAssetNames()) {
			print ("Asset: " + an);
			GameObject redgem = www.assetBundle.LoadAsset (an) as GameObject;

			GameObject rg = Instantiate (redgem) as GameObject;

			RuntimeAnimatorController anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("ScaleGO2", typeof(RuntimeAnimatorController )));
			rg.GetComponent<Animator> ().runtimeAnimatorController = anim;
			gpa.modelAnim = rg.GetComponent<Animator> ();

			rg.transform.SetParent(modelWrapper.transform);
			rg.transform.position = modelWrapper.transform.position;

			rg.AddComponent<RotateGem> ();

			foreach (Transform child in rg.transform) {
				MeshCollider mc = child.gameObject.AddComponent (typeof(MeshCollider)) as MeshCollider;
				child.gameObject.tag = COLLIDER_TAG;
			}
		}
	}

}
