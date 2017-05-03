using UnityEngine;
using UnityEngine.Networking;

public class CharacterController : NetworkBehaviour 
{
	public CharacterModel characterModel;

	public Rigidbody2D rigidBody;
	public Animator animator;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Walk ()
	{
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");

		// Only move in y direction
		if (Mathf.Abs (inputY) >= Mathf.Abs (inputX)) {
			inputX = 0;
		}
		// Only move in x direction
		if (Mathf.Abs (inputX) >= Mathf.Abs (inputY)) {
			inputY = 0;
		}

		bool isWalking = (Mathf.Abs (inputX) + Mathf.Abs (inputY)) > 0;
		animator.SetBool ("isWalking", isWalking);

		// If walking, move in x/y direction
		if (isWalking) {
			animator.SetFloat ("speedX", inputX);
			animator.SetFloat ("speedY", inputY);

			rigidBody.velocity = new Vector2 (inputX * this.characterModel.speed.x, inputY * this.characterModel.speed.y);
		} else {
			rigidBody.velocity = new Vector2 (0, 0);
		}
	}

	public void LastWalk ()
	{
		float lastInputX = Input.GetAxisRaw ("Horizontal");
		float lastInputY = Input.GetAxisRaw ("Vertical");

		// Determine last walk direction
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
}