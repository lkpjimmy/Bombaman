using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharModel : NetworkBehaviour
{
	public CharView charView;

	public float maxHP = 100.0f;
	[SyncVar (hook = "OnHealthChanged")] 
	public float currentHP = 100.0f;
	[SyncVar]
	public Vector2 speed = new Vector2 (100.0f, 100.0f);

	[SyncVar]
	public int activeBombCount = 1;
	[SyncVar]
	public float bombPower = 20f;
	[SyncVar]
	public float bombRadius = 2f;

	[SyncVar]
	public int activeTrapCount = 5;

	[SyncVar]
	public bool isShielded = false;
	[SyncVar]
	public bool isTrapped = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void TakeDamage (float damage)
	{
		// Server's work
		if (!isServer) {
			return;
		}

		this.currentHP -= damage;

		if (this.currentHP <= 0) {
			this.currentHP = 0;
			Destroy (this.gameObject);
			return;
		}
	}

	// Change HP view
	private void OnHealthChanged (float currentHP)
	{
		this.charView.hpBarImage.fillAmount = currentHP / this.maxHP;
	}
		
}
