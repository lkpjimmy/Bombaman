using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Attacker : NetworkBehaviour 
{
	public CharacterController characterController;

	public GameObject bomb;

	[SyncVar]
	public int activeBombCount = 1;
	[SyncVar]
	public float bombPower = 20f;
	[SyncVar]
	public float bombRadius = 0.3f;

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

		if (Input.GetButtonDown ("Jump") && this.activeBombCount >= 1) {
			this.CmdSpawnBomb ();
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
	private void CmdSpawnBomb ()
	{
		Vector3 currentPos = this.transform.position;

		this.bomb.GetComponent<Bomb> ().bombPower = this.bombPower;
		this.bomb.GetComponent<Bomb> ().bombRadius = this.bombRadius;
		GameObject bombObj = Instantiate (this.bomb, currentPos, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (bombObj);

		this.activeBombCount -= 1;
		StartCoroutine (IncreaeBombCount ());
	}

	private void DecreaseBombCount ()
	{
		this.activeBombCount -= 1;
	}

	private IEnumerator IncreaeBombCount ()
	{
		yield return new WaitForSeconds (2.0f);
		this.activeBombCount += 1;
	}
}