using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Attacker : NetworkBehaviour 
{
	public CharModel charModel;
	public CharController charController;

	public GameObject bomb;
	public GameObject trap;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		// Local Player Authority
		if (!isLocalPlayer) {
			return;
		}

		this.charController.Walk ();

		if (Input.GetButtonDown ("SpawnBomb") && this.charModel.activeBombCount >= 1) {
			this.CmdSpawnBomb ();
		}

		if (Input.GetButtonDown ("SpawnTrap") && this.charModel.activeTrapCount >= 1) {
			this.CmdSpawnTrap ();
		}
	}

	void FixedUpdate () 
	{
		if (!isLocalPlayer) {
			return;
		}

		this.charController.LastWalk ();
	}
		
	[Command]
	private void CmdSpawnBomb ()
	{
		Vector3 currentPos = this.transform.position;

		// Instantiate a new bomb
		this.bomb.GetComponent<Bomb> ().bombPower = this.charModel.bombPower;
		this.bomb.GetComponent<Bomb> ().bombRadius = this.charModel.bombRadius;
		GameObject bombObj = Instantiate (this.bomb, currentPos, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (bombObj);
		this.charModel.activeBombCount -= 1;

		StartCoroutine (IncreaeBombCount ());
	}

	private IEnumerator IncreaeBombCount ()
	{
		yield return new WaitForSeconds (2.0f);
		this.charModel.activeBombCount += 1;
	}

	[Command]
	private void CmdSpawnTrap ()
	{
		Vector3 currentPos = this.transform.position;
		if (this.charController.facingDirection == 0) {
			currentPos.y += 26.0f;
		}
		else if (this.charController.facingDirection == 1) {
			currentPos.y -= 26.0f;
		}
		else if (this.charController.facingDirection == 2) {
			currentPos.x -= 26.0f;
		}
		else if (this.charController.facingDirection == 3) {
			currentPos.x += 26.0f;
		}
			
		// Instantiate a new trap
		GameObject trapObj = Instantiate (this.trap, currentPos, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (trapObj);

		this.charModel.activeTrapCount -= 1;
	}

}