using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AttackerController : NetworkBehaviour
{
	public AttackerModel attackerModel;
	public Rigidbody2D rigidBody;
	public Animator animator;

	private float timer = 5f;

	// Use this for initialization
	void Start ()
	{
		this.attackerModel = GetComponent <AttackerModel> ();
		this.rigidBody = GetComponent <Rigidbody2D> ();
		this.animator = GetComponent <Animator>();
	}
		
	// Update is called once per frame
	void Update()
	{
		if (!isLocalPlayer) {
			return;
		}

		this.timer -= Time.deltaTime;

		this.walk ();

		if (Input.GetKey (KeyCode.Space)) {
			this.CmdSpawnBomb ();
		}
	}

	void FixedUpdate () 
	{
		if (!isLocalPlayer) {
			return;
		}

		this.lastWalk ();
	}

	private void walk ()
	{
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");

		// only move in y direction
		if (Mathf.Abs (inputY) >= Mathf.Abs (inputX)) {
			inputX = 0;
		}
		// only move in x direction
		if (Mathf.Abs (inputX) >= Mathf.Abs (inputY)) {
			inputY = 0;
		}

		bool isWalking = (Mathf.Abs (inputX) + Mathf.Abs (inputY)) > 0;
		animator.SetBool ("isWalking", isWalking);

		// if walking, move in x/y direction
		if (isWalking) {
			animator.SetFloat ("speedX", inputX);
			animator.SetFloat ("speedY", inputY);

			rigidBody.velocity = new Vector2 (inputX * attackerModel.speed.x, inputY * attackerModel.speed.y);
		} else {
			rigidBody.velocity = new Vector2 (0, 0);
		}
	}

	private void lastWalk ()
	{
		float lastInputX = Input.GetAxisRaw ("Horizontal");
		float lastInputY = Input.GetAxisRaw ("Vertical");

		// determine last walk direction
		if (lastInputX != 0 || lastInputY != 0) {
			animator.SetBool ("isWalking", true);

			if (lastInputX > 0) {
				animator.SetFloat ("lastMoveX", 1f);
			} else if (lastInputX < 0) {
				animator.SetFloat ("lastMoveX", -1f);
			} else {
				animator.SetFloat ("lastMoveX", 0f);
			}

			if (lastInputY > 0) {
				animator.SetFloat ("lastMoveY", 1f);
			} else if (lastInputY < 0) {
				animator.SetFloat ("lastMoveY", -1f);
			} else {
				animator.SetFloat ("lastMoveY", 0f);
			}
		} else {
			animator.SetBool ("isWalking", false);
		}
	}

	[Command]
	private void CmdSpawnBomb ()
	{
		if (this.timer < 4f && this.attackerModel.activeBombCount > 0) {
			Vector3 currentPos = this.transform.position;
			attackerModel.bomb.GetComponent<Bomb> ().attackerModel = attackerModel;

			GameObject bombObj = Instantiate (attackerModel.bomb, currentPos, Quaternion.identity) as GameObject;
			NetworkServer.Spawn (bombObj);
			attackerModel.activeBombCount -= 1;

			this.timer = 5f;
		}
	}
}