using UnityEngine;
using System.Collections;

public class Brick: MonoBehaviour {
	// Use this for initialization
	public Transform model;
	public Color color = Color.white;
    public int maxLive;
    protected int live;

    protected new ParticleSystem particleSystem;

	public void Start () {
		model.GetComponent<MeshRenderer> ().sharedMaterial.color = color;
        particleSystem = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = color;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ball")
        {
            DoDamage(1);
        }
    }

    public virtual void DoDamage(int damage)
    {
        particleSystem.Emit(30);

        live -= damage;
        if (live <= 0)
        {
            BreakBrick();
        }
    }

    protected virtual void BreakBrick()
    {
        model.GetComponent<MeshRenderer>().enabled = false;
        model.gameObject.SetActive(false);
        
        GetComponent<Collider>().enabled = false;
    }
}

