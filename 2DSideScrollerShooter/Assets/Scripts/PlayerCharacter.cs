using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
	public Attributes stats = new Attributes(); // total stats (items+baseStats) needs updated when equipment changes
	public Attributes items = new Attributes();
	public Attributes baseStats = new Attributes();
	private LootManager lootManager;
	public Transform lootManagerPosition;
	public float bulletSpeed = 10.0f;
	public float timer = 0.0f; 
	public float invTimer;
	public string jumpButton = "Jump";
	public string shootButton = "Fire1";
	public GameObject bulletObject;
	public Transform[] groundChecks;
	public bool invulnerable;
	public AudioClip lootSound;
	public Slider healthSlider;
	private Animator anim;
	public GameObject GUIDamage;
	public GameObject GUIPrefab;


	// Use this for initialization
	void Start () 
	{
		anim = gameObject.GetComponent<Animator> ();
		invulnerable = false;
		invTimer = 0.0f;
		lootManager = GameObject.Find("LootManager").GetComponent("LootManager") as LootManager;
	}
	// Update is called once per frame
	void Update () 
	{
		invTimer += Time.deltaTime;
		if (invTimer > 1.0f) 
		{
			invulnerable = false; 
			//anim.SetBool("Invulnerable", false);
		}
		//Horizontal Movement Logic
		anim.SetFloat("Speed", Mathf.Abs (Input.GetAxis("Horizontal")));
		if(Input.GetAxis("Horizontal") < 0)
		{
			Vector3 newScale = transform.localScale;
			newScale.x = -1.0f;
			transform.localScale = newScale;
		}
		else if(Input.GetAxis("Horizontal") > 0)
		{
			Vector3 newScale = transform.localScale;
			newScale.x = 1.0f;
			transform.localScale = newScale;
		}
		transform.position += transform.right*stats.moveSpeed*Input.GetAxis("Horizontal")*Time.deltaTime;
		//Jump Logic
		bool grounded = false;
		foreach(Transform groundCheck in groundChecks)
			grounded |= Physics2D.Linecast(transform.position, groundCheck.position,
				1 << LayerMask.NameToLayer("Ground"));
		if(Input.GetButtonDown(jumpButton) && grounded)
			rigidbody2D.AddForce(transform.up * stats.jumpHeight);
		//Shoot Logic
		timer += Time.deltaTime;
		if(Input.GetButton(shootButton) && timer > stats.attackSpeed)
		{
			Vector3 direction;
			if(transform.localScale.x >= 0)
				direction = Vector2.right;
			else
				direction = -Vector2.right;
			Quaternion directionQuaternion = Quaternion.LookRotation(direction);
			GameObject bulletInstance = Instantiate(bulletObject, transform.position,
				directionQuaternion) as GameObject;
			timer = 0.0f;
		}
		//NOTE: May want to return player to level map. Player can keep looted items but must pick
		//	a new level to start.
		//Check if player dies
		if(stats.health <= 0)
		{
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		//If player falls below level, reload level
		if(coll.gameObject.tag == "Bounds")
			Application.LoadLevel(Application.loadedLevel);
		if (coll.gameObject.tag == "Enemy" && !invulnerable) 
		{
			stats.health -= 1;
			healthSlider.value = stats.health;
			invulnerable = true; 
			//anim.SetBool("Invulnerable", true);
			invTimer = 0.0f;
		}
		if (coll.gameObject.tag == "Loot") 
		{
			if(coll.gameObject.name == "smallHealth(Clone)")
			{
				AudioSource.PlayClipAtPoint (lootSound, coll.gameObject.transform.position);

				if(stats.health < stats.totalHealth)
				{
					GUIPrefab.GetComponent<GUIText>().color = new Color(0.0f, 255.0f, 0.0f);
					GUIDamage = Instantiate(GUIPrefab, Camera.main.WorldToViewportPoint(gameObject.transform.position), Quaternion.identity) as GameObject;
					GUIDamage.guiText.text = (stats.totalHealth / 10.0f).ToString();
				}

				stats.health += (stats.totalHealth / 10.0f);
				healthSlider.value = stats.health;
				
				Destroy (coll.gameObject);
			}
			else
			if(coll.gameObject.name == "largeHealth(Clone)")
			{

				AudioSource.PlayClipAtPoint (lootSound, coll.gameObject.transform.position);

				if(stats.health < stats.totalHealth)
				{
					float temp = stats.totalHealth - stats.health;
					GUIPrefab.GetComponent<GUIText>().color = new Color(0.0f, 255.0f, 0.0f);
					GUIDamage = Instantiate(GUIPrefab, Camera.main.WorldToViewportPoint(gameObject.transform.position), Quaternion.identity) as GameObject;
					GUIDamage.guiText.text = ((int)temp).ToString();
				}

				stats.health = stats.totalHealth;
				healthSlider.value = stats.health;

				Destroy (coll.gameObject);
			}
			else
			if(coll.gameObject.name != "smallHealth(Clone)" && coll.gameObject.name != "largeHealth(Clone)")
			{
				lootManager.lootItem(coll.gameObject.GetComponent<Item>());
				AudioSource.PlayClipAtPoint (lootSound, coll.gameObject.transform.position);
				coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
				coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
				coll.gameObject.transform.position = lootManagerPosition.position;
			}
		}
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Enemy" && !invulnerable) 
		{
			stats.health -= 1;
			healthSlider.value = stats.health;
			invulnerable = true; 
			invTimer = 0.0f;
		}
	}
}
