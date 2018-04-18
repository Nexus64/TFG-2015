using UnityEngine;
using System.Collections;

public class DivisibleBrick : Brick {
	// Use this for initialization
	public Transform smallBrick;

	protected override void BreakBrick(){
        base.BreakBrick();
		Destroy(model.gameObject);

        Transform fragment = null; 
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j<2; j++)
            {
                fragment = Instantiate(smallBrick);
                fragment.GetComponent<Brick>().color = color;
                fragment.position = transform.position + (new Vector3(-0.6f+i*2, -0.6f+j*2, 0.1f));
            }
        }
		Destroy(gameObject, 2f);
	}
}


