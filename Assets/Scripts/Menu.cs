using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
            SceneManager.LoadScene("stage1");
		}
	}
}
