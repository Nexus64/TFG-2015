using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	public int max_live;
	private int live;
	public int stage_number;
	// Use this for initialization
	void Start () {
		live = max_live;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void do_damage(int damage){
		live -= damage;
		if (live <= 0) {
			string next_stage="stage"+(stage_number+1).ToString();
			if (Application.CanStreamedLevelBeLoaded(next_stage)){
				Application.LoadLevel(stage_number);
			}else{
				Debug.Log ("you win");

			}
		}
	}


}
