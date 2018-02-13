using UnityEngine;
using System.Collections;

public class ExplosionParticle : MonoBehaviour {
	// Use this for initialization
	void Start () {
        ParticleSystem particle_system = GetComponent<ParticleSystem>();
        float duration = particle_system.main.duration;
		Destroy(gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
