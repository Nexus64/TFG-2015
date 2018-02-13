using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class Room : MonoBehaviour {
	public Transform brickPrefab;
	public Transform unbreakablePrefab;
	public Transform squaredPrefab;
	public Transform brickWall;
	public Transform boss;
	public int debugLevel = -1;

	// Use this for initialization
	void Awake () {
		if (debugLevel < 0){
			loadBricks (PlayerPrefs.GetInt ("level"));
		}else if (debugLevel > 9){
			LoadBoss();
		}else{
			loadBricks (debugLevel);
			//PlayerPrefs.SetInt("level", debugLevel);
			PlayerPrefs.Save();
            debugLevel = 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadBoss ()
	{
		Instantiate (boss);
	}

	void loadBricks(int level){
		XmlTextReader reader = (XmlTextReader)XmlTextReader.Create("assets/Resources/map/level"+level.ToString()+".tmx");
		while (reader.Read()) {
			if (reader.Name=="object" && reader.AttributeCount>0){
				//Debug.Log(reader.Name);
				float x=float.Parse(reader.GetAttribute("x"))/16;
				float y=float.Parse(reader.GetAttribute("y"))/16;
				string rstring=reader.GetAttribute("rotation");
				float rotation=0;
				if (rstring!=null){
					rotation=float.Parse(reader.GetAttribute("rotation"));
				}
				int gid=int.Parse(reader.GetAttribute("gid"));

				do{
					reader.Read();
				}while(reader.Name!="property");

                float b = float.Parse(reader.GetAttribute("value"));
                b /= 256;
                do
                {
					reader.Read();
				}while(reader.Name!="property");

				float g=float.Parse(reader.GetAttribute("value"))/256;
				do{
					reader.Read();
				}while(reader.Name!="property");
                float r=float.Parse(reader.GetAttribute("value"))/256;
				Vector3 location=new Vector3(x,16-(y),0);
				Color color=new Color(r,g,b,1);
				switch(gid){
				case 1:
					//Debug.Log(r.ToString()+" "+g.ToString()+" "+b.ToString());
					CreateBrick(location, rotation, color);
					break;
				case 2:
					CreateTripleBrick(location, rotation, color);
					break;
				case 5:
					CreateUnbreakable(location, rotation, color);
					break;
				case 6:
					CreateUnbreakable(location, rotation, color);
					break;
				}
			}
		}
	}

	void CreateBrick(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (brickPrefab);
		brick.transform.SetParent (brickWall);
		brick.localPosition = location+Mathf.Round(Mathf.Sin (rotation))*new Vector3(1,-2,0);
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrick> ().color = color;
	}

	void CreateUnbreakable(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (unbreakablePrefab);
		brick.transform.SetParent (brickWall);
		brick.localPosition = location + new Vector3 (1f, 0.5f, 0);
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrick> ().color = color;
	}

	void CreateTripleBrick(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (brickPrefab);
		brick.transform.SetParent (brickWall);
		brick.localPosition = location+Mathf.Round(Mathf.Sin (rotation))*new Vector3(1,-2,0);
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrick> ().color = color;
		brick.GetComponent<Brick> ().maxLive = 3;
	}
	void CreateSquared(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (squaredPrefab);
		brick.transform.SetParent (brickWall);
		brick.localPosition = location;
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrick> ().color = color;
	}
}