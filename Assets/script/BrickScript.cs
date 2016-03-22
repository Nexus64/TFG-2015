using UnityEngine;
using System.Collections;

public class BrickScript : BasicBrickScript {
	// Use this for initialization
	public int maxLive;
	private int live;
	public void Start () {
		base.Start ();
		live = maxLive;
	}
	void OnCollisionEnter(Collision collision){
		if (collision.transform.tag=="Ball"){
			GetComponent<ParticleSystem>().Emit(30);

			live-=1;
			if (live<=0){
				break_brick();
			}
		}
	}
	void break_brick(){
		Destroy(model.gameObject);
		Destroy(this.gameObject,2f);
	}
}
