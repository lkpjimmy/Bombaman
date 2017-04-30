using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[RequireComponent(typeof(NetworkTransform))]
//[RequireComponent(typeof(Rigidbody))]
public class Attacker : Player
{
	public GameObject bomb;
	public int activeBombCount = 1;

//	private bool isDown;
//	private bool isRight;

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

		if (this.activeBombCount > 0 && Input.GetKey (KeyCode.Space)) {
			this.placeBomb ();
		}
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
	}

	private void placeBomb ()
	{
		Vector3 pos = this.transform.position;
		Instantiate (bomb, pos, Quaternion.identity);
		this.activeBombCount -= 1;
	}
}