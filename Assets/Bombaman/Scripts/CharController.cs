using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CharController : NetworkBehaviour 
{
	public CharModel charModel;
	public CharView charView;

	public Rigidbody2D rigidBody;
	public Animator animator;

	[SyncVar]
	public int facingDirection = 1;

	[SyncVar]
	public float shieldTimer = 10.0f;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.charModel.isShielded == true) {
			this.ShieldTimerCount ();
		}
	}

	public void Walk ()
	{
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");

		// Only move in y direction
		if (Mathf.Abs (inputY) >= Mathf.Abs (inputX)) {
			inputX = 0.0f;
		}
		// Only move in x direction
		if (Mathf.Abs (inputX) >= Mathf.Abs (inputY)) {
			inputY = 0.0f;
		}

		bool isWalking = (Mathf.Abs (inputX) + Mathf.Abs (inputY)) > 0;
		this.animator.SetBool ("isWalking", isWalking);

		// If walking, move in x/y direction
		if (isWalking) {
			this.animator.SetFloat ("speedX", inputX);
			this.animator.SetFloat ("speedY", inputY);

			this.rigidBody.velocity = new Vector2 (inputX * this.charModel.speed.x, inputY * this.charModel.speed.y);

			if (inputY > 0) {
				// Up
				this.facingDirection = 0;
			} else if (inputY < 0) {
				// Down
				this.facingDirection = 1;
			}

			if (inputX > 0) {
				// Right
				this.facingDirection = 3;
			} 
			else if (inputX < 0) {
				// Left
				this.facingDirection = 2;
			}
		} else {
			this.rigidBody.velocity = new Vector2 (0.0f, 0.0f);
		}
	}

	public void LastWalk ()
	{
		float lastInputY = Input.GetAxisRaw ("Vertical");
		float lastInputX = Input.GetAxisRaw ("Horizontal");

		// Determine last walk direction
		if (lastInputY != 0 || lastInputX != 0) {
			this.animator.SetBool ("isWalking", true);

			if (lastInputY > 0) {
				this.animator.SetFloat ("lastMoveY", 1.0f);
			} else if (lastInputY < 0) {
				this.animator.SetFloat ("lastMoveY", -1.0f);
			} else {
				this.animator.SetFloat ("lastMoveY", 0.0f);
			}

			if (lastInputX > 0) {
				this.animator.SetFloat ("lastMoveX", 1.0f);
			} else if (lastInputX < 0) {
				this.animator.SetFloat ("lastMoveX", -1.0f);
			} else {
				this.animator.SetFloat ("lastMoveX", 0.0f);
			}
		} else {
			this.animator.SetBool ("isWalking", false);
		}
	}

	// Trap is a trigger
	private void OnTriggerStay2D (Collider2D other)
	{
		switch (other.gameObject.tag) {
		case "Trap":
			this.charModel.isTrapped = true;
			this.rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
			StartCoroutine (FreeFromTrap (other.gameObject));
			break;
		}
	}

	// Speical items
	private void OnCollisionEnter2D (Collision2D other)
	{
		switch (other.gameObject.tag) {
		case "BombCount":
			this.charModel.activeBombCount += 1;
			Destroy (other.gameObject);
			break;
		case "ExplosionRange":
			this.charModel.bombRadius += 0.1f;
			Destroy (other.gameObject);
			break;
		case "Boot":
			this.charModel.speed += new Vector2 (10.0f, 10.0f);
			Destroy (other.gameObject);
			break;
		case "Shield":
			this.shieldTimer = 10.0f;
			this.charModel.isShielded = true;
			this.charView.shieldImage.enabled = true;
			Destroy (other.gameObject);
			break;
		case "RedPortion":
			if (this.charModel.currentHP + 10.0f <= this.charModel.maxHP && this.charModel.currentHP >= 0) {
				this.charModel.currentHP += 10.0f;
			}
			Destroy (other.gameObject);
			break;	
		case "Poison":
			this.charModel.currentHP -= 10.0f;
			Destroy (other.gameObject);
			break;
		}
	}

	private void ShieldTimerCount ()
	{
		this.shieldTimer -= Time.deltaTime;
		if (this.shieldTimer <= 0.0f) {
			this.shieldTimer = 0.0f;
			this.charModel.isShielded = false;
			this.charView.shieldImage.enabled = false;
		}
	}

	private IEnumerator FreeFromTrap (GameObject trap)
	{
		yield return new WaitForSeconds (3.0f);
		this.charModel.isTrapped = false;
		this.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
		Destroy (trap);
	}
}