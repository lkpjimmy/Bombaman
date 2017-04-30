using UnityEngine;
using System.Collections;

public class GameApplication : MonoBehaviour
{
	public GameArchitecture archi { get { return GameObject.FindObjectOfType<GameArchitecture> (); } }
}

public class GameArchitecture : MonoBehaviour 
{
	public Model model;
	public View view;
	public Controller controller;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
