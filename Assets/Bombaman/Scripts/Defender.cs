using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Defender : Player
{

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate () 
	{

	}

	private void constructBrick ()
	{
		// Vector3 pos = this.transform.position;

		// if (this.timer < 3) {
		// 	Instantiate (block, pos, Quaternion.identity);
		// 	this.timer = 5;
		// }
	}

	private void OnCollisionStay2D (Collision2D other)
	{
		// this.destructObstacle (other);
	}

	private void destructObstacle (Collision2D other)
	{
		// if (Input.GetKey (KeyCode.E)) {
		// 	if (other.gameObject.tag == "Obstacle") {
		// 		Destroy (other.gameObject);
		// 	}
		// }
	}
}