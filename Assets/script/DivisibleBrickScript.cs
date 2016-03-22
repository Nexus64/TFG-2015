using UnityEngine;
using System.Collections;

public class DivisibleBrickScript : BasicBrickScript {
	// Use this for initialization
	public Transform small_brick;
	void OnCollisionEnter(Collision collision){
		if (collision.transform.tag=="Ball"){
			GetComponent<ParticleSystem>().Emit(30);
			Destroy(model.gameObject);
			Transform fragment=null;
			fragment =Instantiate(small_brick);
			fragment.GetComponent<MeshRenderer>().material.color=color;
			fragment.position=transform.position+(new Vector3(0.6f,0.6f,0.1f));
			fragment =Instantiate(small_brick);
			fragment.GetComponent<MeshRenderer>().material.color=color;
			fragment.position=transform.position+(new Vector3(-0.6f,0.6f,0.1f));
			fragment =Instantiate(small_brick);
			fragment.GetComponent<MeshRenderer>().material.color=color;
			fragment.position=transform.position+(new Vector3(0.6f,-0.6f,0.1f));
			fragment =Instantiate(small_brick);
			fragment.GetComponent<MeshRenderer>().material.color=color;
			fragment.position=transform.position+(new Vector3(-0.6f,-0.6f,0.1f));
			Destroy(this.gameObject,2f);

		}
	}
}


