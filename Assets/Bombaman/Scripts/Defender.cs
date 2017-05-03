using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Defender : NetworkBehaviour
{
	public CharacterController characterController;

	public GameObject brick;

	[SyncVar]
	public float brickTimer = 5f;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		this.characterController.Walk ();

		this.brickTimer -= Time.deltaTime;
		if (Input.GetKey (KeyCode.C)) {
			this.CmdConstructBrick ();
		}
	}

	void FixedUpdate () 
	{
		if (!isLocalPlayer) {
			return;
		}

		this.characterController.LastWalk ();
	}

	[Command]
	private void CmdConstructBrick ()
	{
		 Vector3 currentPos = this.transform.position;

		 if (this.brickTimer < 3) {
			GameObject brickObj = Instantiate (this.brick, currentPos, Quaternion.identity) as GameObject;
			NetworkServer.Spawn (brickObj);

		 	this.brickTimer = 5;
		 }
	}

	private void OnCollisionStay2D (Collision2D other)
	{
		 this.CmdDestructObstacle (other);
	}

	[Command]
	private void CmdDestructObstacle (Collision2D other)
	{
		if (Input.GetKey (KeyCode.E)) {
	 		if (other.gameObject.tag == "Obstacle") {

				NetworkServer.Destroy (other.gameObject);
	 		}
	 	}
	}
}