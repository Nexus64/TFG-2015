using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureChangeBrick : Brick {
    public List<Texture> textures = new List<Texture>();
    public int listIndex = 0;

    new Renderer renderer;

	// Use this for initialization
    public new void Start()
    {
        base.Start();
        live = maxLive;
        Transform child = transform.GetChild(0);
        renderer = child.GetComponent<Renderer>();
    }

    public override void DoDamage(int damage) {
        base.DoDamage(damage);
        if (listIndex < textures.Count)
        {
            renderer.material.SetTexture("_MainTex", textures[listIndex]);
            listIndex++;
        }
	}
}
