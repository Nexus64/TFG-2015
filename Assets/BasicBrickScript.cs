using UnityEngine;
using System.Collections;

public class BasicBrickScript: MonoBehaviour {
	// Use this for initialization
	public Transform model;
	public Color color=Color.white;

	public void Start () {
		model.GetComponent<MeshRenderer> ().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

