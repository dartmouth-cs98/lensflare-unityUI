using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

        GameObject canvas = GameObject.FindGameObjectWithTag("SetupFlow");
        canvas.GetComponent<CanvasAnimation>().GrowCanvas();
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("LoadingScene");
    }

}
