using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision collision){
		if (collision.transform.tag=="Ball"){
			Destroy(this.gameObject);
		}
	}
}
