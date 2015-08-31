using UnityEngine;
using System.Collections;
using SimpleJSON;

public class MuseumBuilder : MonoBehaviour {
	//These prefabs hold the basic room shapes. we only need five to construct our museum
	public Transform oneDoorRoom;
	public Transform twoDoorRoom;
	public Transform twoDoorCorner;
	public Transform threeDoorRoom;
	public Transform fourDoorRoom;
	//The size on a side of the square rooms
	public float width = 36;
	//Prefabs for the walls, paintins, and plaques
	public Transform wall;
	public Transform painting;
	public Transform plaque;
	public Transform player;
	//This boolean determines if we look for a museum at the given URL or load it from a text asset
	public bool local=false;
	public TextAsset localMuseum;
	//This is the url we use to get a museum
	public string url = "http://45.55.238.121:8000";
	//The holder for our JSON museum object
	JSONNode N;
	//What have we made yet?
	bool[] instantiated;
	//Everything is asynchronous so that your headset freezes less on startup. It's still not amazing, but  it's alright.
	IEnumerator Start()
	{

		//url = "http://45.55.238.121:8000";
		//Load the museuem from the url
		WWW jsonLoad = new WWW("localhost");
		bool loaded=false;
		if(!local)
		{
			jsonLoad = new WWW(url);
			yield return jsonLoad;
			loaded = (jsonLoad.error ==null);
		}

		//if we didn't crash, or break, or find nothing
		if(loaded || (local && localMuseum!=null))
		{	//we loaded the object, now we parse it as a string
			string jsonString;
			if(local)
			{
				jsonString = localMuseum.text;
			}
			else
			{
				jsonString = jsonLoad.text;
			}
			yield return null;
			//and we feed it to simplejson to parse
			N = JSONNode.Parse(jsonString);
			yield return null;
			//Keep track of which rooms have been instantiated
			instantiated = new bool[N["rooms"].Count];

			//first room
			JSONNode room = N["rooms"][0];
			//Get the room's location (we use a gridded system, so it's integer coordinates
			Vector2 startLocation = new Vector2(room["x"].AsFloat,room["y"].AsFloat);
			//Instantiate the room, and place it in the right spot!
			StartCoroutine(InstantiateRoom (room["room_type"].AsInt,room["rotation"].AsInt, startLocation,room["walls"],0));

		}
		else
		{
			print (jsonLoad.error);
		}
		//InstantiateRoom (4, 90, new Vector2 (0, 0), new Vector2(0,0),new Vector2(1,1), "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Instantiates, rotates, and moves room to its position
	IEnumerator InstantiateRoom(int roomType, int roomRotation, Vector2 location, JSONNode walls, int index)
	{
		Transform room;
		if(roomType==1){
			room = Instantiate(oneDoorRoom) as Transform;
		}
		else if(roomType==3){
			room = Instantiate(twoDoorRoom) as Transform;
		}
		else if(roomType==2){
			room = Instantiate(twoDoorCorner) as Transform;
		}
		else if(roomType==4){
			//print ("no it's happening");
			room = Instantiate(threeDoorRoom) as Transform;
		}
		else
		{
			room = Instantiate(fourDoorRoom) as Transform;
		}

		//Makes each of the walls that hold a single painting.
		for(int i=0;i<walls.Count;i++)
		{	
			Vector2 start = new Vector2(walls[i]["startx"].AsFloat,walls[i]["starty"].AsFloat);
			Vector2 end = new Vector2(walls[i]["endx"].AsFloat,walls[i]["endy"].AsFloat);
			Transform newWall =	MakeWall (start, end);
			newWall.SetParent (room);
			//Instantiate a painting, and attach the image it needs from the url given.
			Transform newPainting = Instantiate (painting) as Transform;
			PaintingScript ps = newPainting.GetComponent<MonoBehaviour>() as PaintingScript;
			ps.ResizePainting(new Vector2(newWall.localScale.x,newWall.localScale.y), walls[i]["url"].ToString());

			
			float wallLength = (start - end).magnitude*width;

			//We put plaques that float next to the painting, so you can see who made it and what it's called.
			Transform newPlaque = Instantiate (plaque) as Transform;
			newPlaque.position= newWall.position+newWall.transform.forward*.3f - newWall.transform.right*wallLength*.25f;
			newPainting.position = newWall.position+newWall.transform.forward*.3f;
			//This is all basically voodoo to make the text appear in the right spot, with the right rotation
			TextMesh tm = newPlaque.GetComponent<TextMesh>();
			tm.text = walls[i]["descr"];
			tm.text = tm.text.Replace("by ","\n");
			newPlaque.rotation= newWall.rotation;
			newPlaque.Rotate (new Vector3(0,180,0));
			newPlaque.SetParent (room);
			newPainting.rotation = newWall.rotation;
			newPainting.SetParent(room);
			yield return new WaitForSeconds(.1f);
		}

		//Transform room. We always instantiate and build them at the origin for ease of use, then move them to the right spot.
		room.Rotate(Vector3.up*roomRotation);
		Vector3 movement = new Vector3(width*location.x,0,width*location.y);
		room.position = room.position + movement;
		if(index<N["rooms"].Count-1)
		{
			JSONNode nextRoom = N["rooms"][index+1];
			Vector2 startLocation = new Vector2(nextRoom["x"].AsFloat,nextRoom["y"].AsFloat);
			//make the next room
			StartCoroutine(InstantiateRoom (nextRoom["room_type"].AsInt,nextRoom["rotation"].AsInt, startLocation,nextRoom["walls"],index+1));
		}
	}	

	Transform MakeWall(Vector2 start, Vector2 end)
	{
		Transform newWall = Instantiate (wall) as Transform;
		float wallCenterX = ((start.x + end.x) / 2.0f);
		float wallCenterY = ((start.y + end.y) / 2.0f);
		//calculate length
		float wallLength = (start - end).magnitude*width;
		//calculate location
		Vector2 wallCenter = new Vector2 (wallCenterX, wallCenterY);
		//calculate angle
		float angle1 =Mathf.PI+ Mathf.Atan2(start.y-end.y,
		                           start.x-end.x);

		///float angle2 = Mathf.Atan2(line2.getY1() - line2.getY2(),
		   //                        line2.getX1() - line2.getX2());
		Vector3 wallRotation =(180.0f/Mathf.PI)*angle1*newWall.up;//(180.0f/Mathf.PI)*(Mathf.Atan(2*(start.y-wallCenterY)/(wallLength)))*newWall.up;
		//print (angle1);
		wallCenterX -= 0.5f;
		wallCenterY = -(wallCenterY - 0.5f);


		//Transform wall;
		newWall.Rotate(wallRotation);
		Vector3 movement = new Vector3(width*wallCenterX,0,width*wallCenterY);
		newWall.position = newWall.position + movement;
		newWall.localScale = new Vector3 (wallLength, newWall.localScale.y, newWall.localScale.z);

		return newWall;
	}
}