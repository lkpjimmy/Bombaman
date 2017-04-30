using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public Rigidbody2D rigidBody;
	public Animator anim;

	public Vector2 speed = new Vector2(40, 40);
	public float hp = 100;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void walk ()
	{
		float inputX = Input.GetAxisRaw ("Horizontal");
		float inputY = Input.GetAxisRaw ("Vertical");

		if (Mathf.Abs (inputY) >= Mathf.Abs (inputX)) {
			inputX = 0;
		}

		if (Mathf.Abs (inputX) >= Mathf.Abs (inputY)) {
			inputY = 0;
		}

		bool isWalking = (Mathf.Abs (inputX) + Mathf.Abs (inputY)) > 0;

		anim.SetBool ("isWalking", isWalking);

		if (isWalking) {
			anim.SetFloat ("speedX", inputX);
			anim.SetFloat ("speedY", inputY);

			rigidBody.velocity = new Vector2 (inputX * speed.x, inputY * speed.y);
		} else {
			rigidBody.velocity = new Vector2 (0, 0);
		}
	}

	public void lastWalk ()
	{
		float lastInputX = Input.GetAxisRaw ("Horizontal");
		float lastInputY = Input.GetAxisRaw ("Vertical");

		if (lastInputX != 0 || lastInputY != 0) {
			anim.SetBool ("isWalking", true);


			if (lastInputX > 0) {
				anim.SetFloat ("lastMoveX", 1f);
			} else if (lastInputX < 0) {
				anim.SetFloat ("lastMoveX", -1f);
			} else {
				anim.SetFloat ("lastMoveX", 0f);
			}

			if (lastInputY > 0) {
				anim.SetFloat ("lastMoveY", 1f);
			} else if (lastInputY < 0) {
				anim.SetFloat ("lastMoveY", -1f);
			} else {
				anim.SetFloat ("lastMoveY", 0f);
			}
		} else {
			anim.SetBool ("isWalking", false);
		}
	}
}
