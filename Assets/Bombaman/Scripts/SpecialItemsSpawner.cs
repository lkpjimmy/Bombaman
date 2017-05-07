using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SpecialItemsSpawner : NetworkBehaviour 
{
	public GameObject[] specialItems;
	public const int maxItemCount = 6;

	[SyncVar]
	public float gameTimer = 0.0f;

	[SyncVar]
	public float spawnerTimer = 5.0f;
	[SyncVar]
	public float spawnerTimerValue = 5.0f;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		this.gameTimer += Time.deltaTime;
		if (this.gameTimer >= 60.0f) {
			this.spawnerTimerValue = 4.0f;
		} 
		else if (this.gameTimer >= 120.0f) {
			this.spawnerTimerValue = 3.0f;	
		}
		else if (this.gameTimer >= 180.0f) {
			this.spawnerTimerValue = 2.0f;	
		}
		else if (this.gameTimer >= 240.0f) {
			this.spawnerTimerValue = 1.0f;	
		}

		this.spawnerTimer -= Time.deltaTime;
		this.CmdSpawnItem ();
	}
		
	private int GetRandomItemType ()
	{
		Random.seed = System.Guid.NewGuid ().GetHashCode ();
		int randItemType = Random.Range (0, maxItemCount);
		return randItemType;
	}

	private Vector3 GetRandomPosition ()
	{
		Random.seed = System.Guid.NewGuid ().GetHashCode ();
		int coordinateX = Random.Range (-180, 180);
		int coordinateY = Random.Range (-180, 180);

		Vector3 randPos = new Vector3 (coordinateX, coordinateY, 0);

		return randPos;
	}

	[Command]
	private void CmdSpawnItem ()
	{
		if (this.spawnerTimer <= 0.0f) {
			this.spawnerTimer = this.spawnerTimerValue;

			int itemType = this.GetRandomItemType ();
			Vector3 pos = this.GetRandomPosition ();

			GameObject specialItemObj = Instantiate (specialItems [itemType], pos, Quaternion.identity) as GameObject;
			NetworkServer.Spawn (specialItemObj);

			StartCoroutine (DestroyItem (specialItemObj));
		}
	}

	private IEnumerator DestroyItem (GameObject specialItemObj)
	{
		yield return new WaitForSeconds (5.0f);
		NetworkServer.Destroy (specialItemObj);
	}
}