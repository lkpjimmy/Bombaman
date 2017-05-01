using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AttackerModel : NetworkBehaviour
{
	public float hp = 100;
	public Vector2 speed = new Vector2(50, 50);

	public int activeBombCount = 1;
	public GameObject bomb;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}