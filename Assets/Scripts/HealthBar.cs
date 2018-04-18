using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
    public float maxHealth;
    public RectTransform healthTransform;

	private float minXValue;
	private float maxXValue;
	private int currentHealth;
	
	// Update is called once per frame
	void Update () {
		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

        healthTransform.localPosition = new Vector2 (currentXValue, healthTransform.localPosition.y);
	}
	
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
	
	public void DecreaseLife(float damage){
		currentHealth -= (int) damage;
	}

    public void SetHealth(float newHealth)
    {
        maxHealth = newHealth;
        currentHealth = (int)(int)maxHealth; ;
        maxXValue = healthTransform.localPosition.x;
        minXValue = healthTransform.localPosition.x - healthTransform.rect.width;
        print(healthTransform.rect);
    }
}
