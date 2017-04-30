using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[RequireComponent(typeof(NetworkTransform))]
//[RequireComponent(typeof(Rigidbody))]
public class Defender : Player
{
	public GameObject block;
	private float timer = 3;

	// Use this for initialization
	void Start ()
	{
		this.rigidBody = GetComponent <Rigidbody2D> ();
		this.anim = GetComponent <Animator>();
		this.speed = new Vector2(40, 40);
		this.hp = 100;
	}

	// Update is called once per frame
	//	[ClientCallback]
	void Update()
	{
//		if (!isLocalPlayer) {
//			return;
//		}
		this.walk ();

		this.timer -= Time.deltaTime;
	}
	//		
	//	[ClientCallback]
	//
	void FixedUpdate () 
	{
//		if (!isLocalPlayer) {
//			return;
//		}
		this.lastWalk ();

		if (Input.GetKey (KeyCode.C)) {
			this.constructBrick ();
		}
	}

	private void constructBrick ()
	{
		Vector3 pos = this.transform.position;

		if (this.timer < 3) {
			Instantiate (block, pos, Quaternion.identity);
			this.timer = 5;
		}
	}

	private void OnCollisionStay2D (Collision2D other)
	{
		this.destructObstacle (other);
	}

	private void destructObstacle (Collision2D other)
	{
		if (Input.GetKey (KeyCode.E)) {
			if (other.gameObject.tag == "Obstacle") {
				Destroy (other.gameObject);
			}
		}
	}
}