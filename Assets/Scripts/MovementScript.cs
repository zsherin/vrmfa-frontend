using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	public Rigidbody rb;
	public Camera camera;
	bool moving;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(0,-4.4f,7);
		moving = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		RaycastHit hit;
		Ray ray = new Ray(transform.position,camera.transform.forward);
		//Debug.DrawRay (ray);

		rb.velocity = Vector3.zero;
		Debug.DrawRay (ray.origin,ray.direction);
		if (Physics.Raycast(ray, out hit)) {
			Transform objectHit = hit.transform;
			//print ("HELLO");
			
			if(objectHit.tag == "MoveSquare")
			{
				Vector3 velocity = 10* camera.transform.forward;
				velocity.y = 0;
				rb.velocity=velocity;
				print ("HEYO");
			}
		
		}
		*/if (Cardboard.SDK.CardboardTriggered) {
			moving = !moving;
		}
		if(moving)
		{
			Vector3 movement = camera.transform.forward;
			movement.y=0;
			transform.position+=movement*.2f;
		}
	}
}
