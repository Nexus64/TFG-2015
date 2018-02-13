using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CustomImageEffect : MonoBehaviour {

	public Material effect_material;

	void OnRenderImage(RenderTexture src, RenderTexture dts) {
		Graphics.Blit(src, dts, effect_material);
	}
}
