using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bomb : NetworkBehaviour
{
	public AttackerModel attackerModel;
	
	public GameObject explosion;
	public CircleCollider2D circleCollider;

	public float power = 20f;

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
		Debug.Log (NetworkServer.active);

		GameObject explosionObj = Instantiate (explosion, this.transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn (explosionObj);

		circleCollider.enabled = true;
	
		Invoke ("destroyBomb", 0.5f);
	}
		
	// When bomb explodes, interact with surrounding
	private void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Brick") {
			StartCoroutine (destroyBrick (other));
		} 

		else if (other.gameObject.tag != "Player") {
			other.gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
			other.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (2000, 2000));
		}
	}
		
	private void destroyBomb ()
	{
		Destroy (this.gameObject);
		attackerModel.activeBombCount += 1;
	}

	private IEnumerator destroyBrick (Collision2D other)
	{
		yield return new WaitForSeconds (0.0f);
		other.gameObject.SetActive (false);
	}
}