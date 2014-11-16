using UnityEngine;
using System.Collections;

public class DestroyOnInvisible : MonoBehaviour {

	public GameObject destroyTarget = null;
	public GameObject GUIDamage;
	public GameObject GUIPrefab;

	Enemy enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameInvisible() 
	{

		if (destroyTarget == null)
		{
			Destroy (gameObject);
		} 
		else 
		{
			Destroy(destroyTarget);
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{

		if(coll.gameObject.tag == "Enemy")
		{
			enemy = coll.gameObject.GetComponent<Enemy>();

			GUIPrefab.GetComponent<GUIText>().color = new Color(255.0f, 255.0f, 255.0f);
			GUIDamage = Instantiate(GUIPrefab, Camera.main.WorldToViewportPoint(gameObject.transform.position), Quaternion.identity) as GameObject;
			GUIDamage.guiText.text = "10";

			enemy.stats.health -= 10;

			if (destroyTarget == null)
			{
				Destroy (gameObject);
			} 
			else 
			{
				Destroy(destroyTarget);
			}
		}

		if(coll.gameObject.tag == "Ground")
		{
			if (destroyTarget == null)
			{
				Destroy (gameObject);
			} 
			else 
			{
				Destroy(destroyTarget);
			}
		}
	}
}
