using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGUI : MonoBehaviour {
    Animator animator;

	// Use this for initialization
	void Awake () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void SetVisible(bool value)
    {
        animator.SetBool("Visible", value);
    }

    public void StartFade()
    {
        animator.SetBool("Faded", true);
    }
}
