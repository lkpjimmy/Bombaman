using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharModel : NetworkBehaviour
{
	[SyncVar (hook = "OnHealthChanged")] public float currentHP = 100f;
	public float maxHP = 100f;
	public Vector2 speed = new Vector2 (100f, 100f);

	public Image hpBar;

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
		if (!isServer) {
			return;
		}

		this.currentHP -= damage;
	}

	private void OnHealthChanged (float currentHP)
	{
		this.hpBar.fillAmount = currentHP / this.maxHP;
	}
}
