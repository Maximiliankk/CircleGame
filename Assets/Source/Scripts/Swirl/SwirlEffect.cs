using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwirlEffect : MonoBehaviour {

    public Material swirlMaterial;

	void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, swirlMaterial);
    }
}
