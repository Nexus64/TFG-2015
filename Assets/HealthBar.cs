using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	private float cachedY;
	private float minXValue;
	private float maxXValue;
	private int currentHealth;
	public float maxHealth;
	public Image visualHealth;
	// Use this for initialization
	void Start () {
		cachedY = transform.position.y;
		maxXValue = transform.position.x;
		minXValue = transform.position.x - GetComponent<RectTransform> ().rect.width;
		currentHealth = (int)maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);
		visualHealth.transform.position = new Vector2 (currentXValue, cachedY);
	}
	
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
	
	public void decreaseLife(float damage){
		currentHealth -= (int) damage;
	}
}
