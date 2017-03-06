using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour {

    public void Change(string name)
    {
        Texture tex = Resources.Load(name) as Texture;
        Renderer rend = GetComponent<Renderer>();
        rend.material.mainTexture = tex;

    }
}
