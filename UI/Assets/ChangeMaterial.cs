using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour {

    public void Change(string name)
    {
        print("name: " + name);
        Texture tex = Resources.Load(name) as Texture;
        print(tex);
        Renderer rend = GetComponent<Renderer>();
        rend.material.mainTexture = tex;

    }
}
 