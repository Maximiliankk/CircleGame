using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlRenderer : MonoBehaviour {

    private Material mat;

	void Awake()
    {
        mat = Resources.Load("SwirlMaterial") as Material;
    }

    void Update()
    {
        Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
        mat.SetVector("_Center", new Vector4(pos.x, pos.y));
    }
}
