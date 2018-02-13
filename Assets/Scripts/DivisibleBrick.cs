using UnityEngine;
using System.Collections;

public class DivisibleBrick : BasicBrick {
	// Use this for initialization
	public Transform smallBrick;

	void OnCollisionEnter(Collision collision){
		if (collision.transform.tag=="Ball"){
			GetComponent<ParticleSystem>().Emit(30);
			Destroy(model.gameObject);

            Transform fragment = null; 
            for (int i = 0; i<2; i++)
            {
                for (int j = 0; j<2; j++)
                {
                    fragment = Instantiate(smallBrick);
                    fragment.GetComponent<Brick>().color = color;
                    fragment.position = transform.position + (new Vector3(-0.6f+i*2, -0.6f+j*2, 0.1f));
                }
            }
			Destroy(this.gameObject, 2f);
		}
	}
}


