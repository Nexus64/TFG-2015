using UnityEngine;
using System.Collections;

public class BasicBrick: MonoBehaviour {
	// Use this for initialization
	public Transform model;
	public Color color=Color.white;
    public int maxLive;
    protected int live;

    private new ParticleSystem particleSystem;

	public void Start () {
		model.GetComponent<MeshRenderer> ().material.color = color;
        particleSystem = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = color;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoDamage(int damage)
    {
        particleSystem.Emit(30);

        live -= damage;
        if (live <= 0)
        {
            model.gameObject.SetActive(false);
            BreakBrick();
        }
    }

    void BreakBrick()
    {
        //Destroy(model.gameObject);
        model.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}

