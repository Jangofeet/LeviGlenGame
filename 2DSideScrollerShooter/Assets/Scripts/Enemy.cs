using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public Attributes stats = new Attributes ();
	public float lootForce = 500.0f;
	public GameObject loot;
	Item item = new Item();

	// Use this for initialization
	void Start () 
	{
		stats.set(1,0,30.0f,30.0f,0.0f,4.0f,0.5f,400.0f,1.0f);
	}
	// Update is called once per frame
	void Update () 
	{
		if(stats.health <= 0)
		{
			Destroy(gameObject);
			item.generateLoot(stats.level, Random.Range (0,5), Random.Range(0,6), Random.Range(0,6),
				gameObject.transform);
		}
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		//If player falls below level, reload level
		if(coll.gameObject.tag == "Bullet")
		{

		}
	}
}
