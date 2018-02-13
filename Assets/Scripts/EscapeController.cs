using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeController : MonoBehaviour {
    public static EscapeController controller;
	// Use this for initialization
	void Start () {
		if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
	}
}
