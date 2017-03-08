using System.Collections;
using UnityEngine;

public class LoadAssets : MonoBehaviour {

	public string COLLIDER_TAG;
    public GameObject modelWrapper;
	public GemPrefabAnimator gpa;

    string previous_url = null; 
    public void DownloadModel(string url)
    {
        StartCoroutine(Load(url));
    }

	public IEnumerator Load(string url) {

        if (url != previous_url)
        {
            WWW www = WWW.LoadFromCacheOrDownload(url, 1);
            yield return www;

            foreach (string an in www.assetBundle.GetAllAssetNames())
            {
                print("Asset: " + an);
                GameObject redgem = www.assetBundle.LoadAsset(an) as GameObject;

                GameObject rg = Instantiate(redgem) as GameObject;

                RuntimeAnimatorController anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("ScaleGO2", typeof(RuntimeAnimatorController)));
                rg.GetComponent<Animator>().runtimeAnimatorController = anim;
                gpa.modelAnim = rg.GetComponent<Animator>();

                rg.transform.SetParent(modelWrapper.transform);

                if (modelWrapper.transform.childCount != 0)
                {
                    Transform prev = modelWrapper.transform.GetChild(0);
                    if (prev != null) { 
                        Destroy(prev);
                    }
                }
               

                rg.transform.position = modelWrapper.transform.position;

                rg.AddComponent<RotateGem>();

                foreach (Transform child in rg.transform)
                {
                    MeshCollider mc = child.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                    child.gameObject.tag = COLLIDER_TAG;
                }
            }
        }
        previous_url = url;
        gpa.modelAnim.SetTrigger("StateChange");
		
	}

}
