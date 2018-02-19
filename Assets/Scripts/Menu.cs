using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class Menu : MonoBehaviour {
    public int gameRoomID;
    bool inTransition = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space)) {
            print("SPACE PRESSED");
            if (!inTransition) {
                FlashTransition transition = Camera.main.GetComponent<FlashTransition>();

                transition.StartTransition(gameRoomID);
                inTransition = true;
            }
        }
	}
}
