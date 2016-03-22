﻿using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	public int max_live;
	private int live;
	public int stage_number;
	public HealthBar health;
	// Use this for initialization
	void Start () {
		live = max_live;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void do_damage(int damage){
		live -= damage;
		health.decreaseLife (10);
		if (live <= 0) {
			string next_stage="stage"+(stage_number+1).ToString();
			Debug.Log (next_stage);
			if (Application.CanStreamedLevelBeLoaded(next_stage)){
				Application.LoadLevel(next_stage);
			}else{
				Debug.Log ("you win");

			}
		}
	}


}