using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class RoomScript : MonoBehaviour {
	public Transform brick_prefab;
	public Transform unbreakable_prefab;
	public Transform squared_prefab;
	public Transform brick_wall;
	public int debug_level;
	// Use this for initialization
	void Awake () {
		if (debug_level < 0){
			load_bricks (PlayerPrefs.GetInt ("level"));
		}else{
			load_bricks (debug_level);
			PlayerPrefs.SetInt("level",debug_level);
			PlayerPrefs.Save();
			debug_level=0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void load_bricks(int level){

		XmlTextReader reader = (XmlTextReader)XmlTextReader.Create("assets/Resources/map/level"+level.ToString()+".tmx");
		while (reader.Read()) {
			if (reader.Name=="object" && reader.AttributeCount>0){
				Debug.Log(reader.Name);
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

				float b=float.Parse(reader.GetAttribute("value"))/256;
				do{
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
					Debug.Log(r.ToString()+" "+g.ToString()+" "+b.ToString());
					create_brick(location, rotation, color);
					break;
				case 2:
					create_triple_brick(location, rotation, color);
					break;
				case 5:
					create_unbreakable(location, rotation, color);
					break;
				case 6:
					create_unbreakable(location, rotation, color);
					break;
				}

			}
		}

	}

	void create_brick(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (brick_prefab);
		brick.transform.SetParent (brick_wall);
		brick.localPosition = location+Mathf.Round(Mathf.Sin (rotation))*new Vector3(1,-2,0);
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrickScript> ().color = color;
	}
	void create_unbreakable(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (unbreakable_prefab);
		brick.transform.SetParent (brick_wall);
		brick.localPosition = location + new Vector3 (1f, 0.5f, 0);
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrickScript> ().color = color;
	}
	void create_triple_brick(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (brick_prefab);
		brick.transform.SetParent (brick_wall);
		brick.localPosition = location+Mathf.Round(Mathf.Sin (rotation))*new Vector3(1,-2,0);
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrickScript> ().color = color;
		brick.GetComponent<BrickScript> ().maxLive = 3;
	}
	void create_squared(Vector3 location, float rotation, Color color){
		Transform brick = Instantiate (squared_prefab);
		brick.transform.SetParent (brick_wall);
		brick.localPosition = location;
		brick.rotation=Quaternion.Euler(new Vector3(0,0,rotation));
		brick.GetComponent<BasicBrickScript> ().color = color;
	}
}