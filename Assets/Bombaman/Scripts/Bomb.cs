using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bomb : NetworkBehaviour
{
	public GameObject explosion;
	public CircleCollider2D circleCollider;

	public float bombPower = 20f;
	public float bombRadius = 0.3f;

	// Start bomb explosion in 2 seconds
	void Start () 
	{ 
		Invoke ("CmdSpawnExplosion", 2.0f);
	}

	// Update is called once per frame
	void Update ()
	{

	}
		
	[Command]
	private void CmdSpawnExplosion ()
	{
		GameObject explosionObj = Instantiate (this.explosion, this.transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (explosionObj);

		circleCollider.enabled = true;
	
		Invoke ("DestroyBomb", 0.5f);
	}
		
	private void DestroyBomb ()
	{
		Destroy (this.gameObject);
	}

	// When bomb explodes, interact with surrounding
	private void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Brick") {
			StartCoroutine (DestroyBrick (other));
		}
		else if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<CharacterModel> ().TakeDamage (this.bombPower);
		}
		else {
			other.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (2000, 2000));
		}
	}

	private IEnumerator DestroyBrick (Collision2D other)
	{
		yield return new WaitForSeconds (0.0f);
		other.gameObject.SetActive (false);
	}
}