using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour 
{
	public Model model;
	public CircleCollider2D circleCollider;
	public GameObject explosion;

	// start bomb explosion immediately
	void Start () 
	{ 
		model = GameObject.Find ("Model").GetComponent <Model> ();
		StartCoroutine (playExplosion ());
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private IEnumerator playExplosion ()
	{
		yield return new WaitForSeconds (2.0f);
		Instantiate (explosion, this.transform.position, Quaternion.identity);
		circleCollider.enabled = true;
	
		StartCoroutine (destroyBomb ());
	}

	// when bomb explode, interaction with surrounding
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

	private IEnumerator destroyBrick (Collision2D other)
	{
		yield return new WaitForSeconds (0.0f);
		other.gameObject.SetActive (false);
	}

	private IEnumerator destroyBomb ()
	{
		yield return new WaitForSeconds (0.5f);
		model.changeActiveBombCount (1);
		Destroy (this.gameObject);
	}
}