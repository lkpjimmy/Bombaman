using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bomb : NetworkBehaviour
{	
	public GameObject explosion;
	public CircleCollider2D circleCollider2D;

	[SyncVar]
	public float bombPower = 20f;
	[SyncVar]
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
		this.explosion.transform.localScale = new Vector3 (this.bombRadius * 30.0f, this.bombRadius * 30.0f, 1.0f);
		this.circleCollider2D.radius = this.bombRadius;
		this.circleCollider2D.enabled = true;

		GameObject explosionObj = Instantiate (this.explosion, this.transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (explosionObj);
	
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
		else if (other.gameObject.tag == "Obstacle") {
			other.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (2000.0f, 2000.0f));
		} 
		else if (other.gameObject.tag == "Player") {
			if (other.gameObject.GetComponent<CharModel> ().isShielded == false) {
				other.gameObject.GetComponent<CharModel> ().TakeDamage (this.bombPower);
			}
		}
	}

	private IEnumerator DestroyBrick (Collision2D other)
	{
		yield return new WaitForSeconds (0.0f);
		other.gameObject.SetActive (false);
	}
}