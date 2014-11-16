using UnityEngine;
using System.Collections;
/*
   Attributes Class:
		This class includes all the attributes needed for the character, items, enemies, and bosses.
		Use enforceLimits() to provide level increases. Just apply experience and it will add levels.
		Ensure all objects in the game reference these attributes, do not create rogue attributes for
		any reason.
	Author: Brian J. Glenn
	Date: 2014-11-14
*/
public class Attributes : MonoBehaviour
{
	public int level;
	public int experience;
	public float health; // low is bad
	public float totalHealth;
	public float healthRegen;
	public float attack;
	public float attackSpeed; // number per second
	public float jumpHeight;
	public float moveSpeed;

	public void set(int level, int experience, float health, float totalHealth, float healthRegen,
		float attack,float attackSpeed, float jumpHeight, float moveSpeed)
	{
		this.level = level;
		this.experience = experience;
		this.health = health;
		this.totalHealth = totalHealth;
		this.healthRegen = healthRegen;
		this.attack = attack;
		this.attackSpeed = attackSpeed;
		this.jumpHeight = jumpHeight;
		this.moveSpeed = moveSpeed;
	}
	public void setZero()
	{
		this.set(0,0,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f,0.0f);
	}
	public void applyBaseStats()
	{
		level = 1;
		experience = 0;
		health = 10.0f;
		totalHealth = 10.0f;
		healthRegen = 0.0f;
		attack = 4.0f;
		attackSpeed = 0.5f;
		jumpHeight = 400.0f;
		moveSpeed = 5.0f;
	}
	// if any attributes fall outside of their intended range, clamp those values
	public void enforceLimits()
	{
		if(experience < 0)
			experience = 0;
		if(experience >= 1000)
		{
			level += experience/1000;
			experience %= 1000;
		}
		if(level < 1)
			level = 1;
		if(level > 100)
			level = 100;
		if(health < 0.0f)
			health = 0.0f;
		if(totalHealth < 1.0f)
			totalHealth = 1.0f;
		if(healthRegen < 0.0f)
			healthRegen = 0.0f;
		if(healthRegen > totalHealth*0.1f)
			healthRegen = totalHealth*0.1f;
		if(attack < 1.0f)
			attack = 1.0f;
		if(jumpHeight < 1.0f)
			jumpHeight = 1.0f;
		if(moveSpeed < 0.1f)
			moveSpeed = 0.1f;
	}
	void Start()
	{
		applyBaseStats();
		enforceLimits();
	}
	void Update()
	{

	}
}
