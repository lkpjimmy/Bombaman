using UnityEngine;

public class Model : MonoBehaviour 
{

	public GameObject attacker;

	// Use this for initialization
	void Start () 
	{
				
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void changeActiveBombCount (int amount)
	{
		attacker.GetComponent<Attacker>().activeBombCount += amount;	
	}
}