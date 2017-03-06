using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public Animator canvasAnim; 

    // Use this for initialization
    void Start()
    {
        canvasAnim.SetTrigger("StateChange");
        StartCoroutine(Example(6));
    }

    IEnumerator Example(int dur)
    {
        yield return new WaitForSeconds(6);
        canvasAnim.SetTrigger("StateChange");
        StartCoroutine(ShortWait(2));
    }

    IEnumerator ShortWait(int dur)
    { 
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("LoadingScene");

    }

}
