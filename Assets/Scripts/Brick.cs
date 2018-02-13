using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brick : BasicBrick {
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

    void OnCollisionEnter(Collision collision){
		if (collision.transform.tag=="Ball"){
            DoDamage(1);
            if (listIndex < textures.Count)
            {
                renderer.material.SetTexture("_MainTex", textures[listIndex]);
                listIndex++;
            }
        }
	}
}
