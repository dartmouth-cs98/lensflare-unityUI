using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssets : MonoBehaviour {

	public string COLLIDER_TAG;
//	public GameObject gem;
	public GameObject modelWrapper;

	public GemPrefabAnimator gpa; 

	public string modelUrl;


	public IEnumerator Load() {
		WWW www = WWW.LoadFromCacheOrDownload ("file:///Users/armin/Desktop/redgem" ,1);
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
