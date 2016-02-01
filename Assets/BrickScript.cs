using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {
	// Use this for initialization
	public Transform model;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision){
		if (collision.transform.tag=="Ball"){
			GetComponent<ParticleSystem>().Emit(30);
			Destroy(model.gameObject);
			Destroy(this.gameObject,2f);

		}
	}
}
