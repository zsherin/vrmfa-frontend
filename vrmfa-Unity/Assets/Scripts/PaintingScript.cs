using UnityEngine;
using System.Collections;

public class PaintingScript : MonoBehaviour {
	public string url = "";
	public MeshRenderer render;
	Vector2 imageSize;
	///public WWW www;
	public float frameWidth;
	// Use this for initialization
	IEnumerator  Start () {
		
		render = this.GetComponent<MeshRenderer>();
		if(url != "")
		{
			Debug.Log ("hey dogg");
			WWW www = new WWW(url);
			yield return www;	
			render.material.mainTexture = www.texture;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	IEnumerator loadImage(string newUrl,Vector2 wallSize)
	{
	
		newUrl=newUrl.Replace("\"", "");

		WWW www = new WWW(newUrl);
		yield return www;
		//www.texture.Resize(512,512);
		Texture2D tex = new Texture2D(256,256);
		www.LoadImageIntoTexture(tex);
		imageSize = new Vector2(www.texture.width,www.texture.height);

		float ratio = imageSize.x/imageSize.y;
		float paintingMaxRatio = .33f;

		
		tex.Compress(false);

		render.material.mainTexture = tex;

		//print (imageSize);

		float newY = wallSize.y * 2/3.0f;
		float newX = newY*ratio;
		//print (ratio);
		//print (newX);
		//print(newY);
		if(newX > wallSize.x*paintingMaxRatio)
		{
			newX = wallSize.x*paintingMaxRatio;
			newY = newX/(ratio);
		}
		
		transform.localScale = new Vector3( newX,newY,transform.localScale.z);
		//print ("hey");
		// frame scaling
		float frameScaleX, frameScaleY;
		frameScaleX = (newX + 2 * frameWidth)/newX;
		frameScaleY = (newY + 2 * frameWidth)/newY;
		transform.GetChild (0).localScale = new Vector3 (frameScaleX, frameScaleY, .8f);

	}

	public void ResizePainting(Vector2 wallSize, string newUrl)
	{
		render = this.GetComponent<MeshRenderer>();
		StartCoroutine(loadImage (newUrl,wallSize));



	}
}



