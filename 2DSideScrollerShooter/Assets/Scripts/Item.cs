using UnityEngine;
using System.Collections;
/*
	Item Class:
		A robust item generation class. Can be randomized by placing random numbers within
		the usable range in the generate method.
		cost: will try to closely match item to this price. +15*((25+totalPurchased)/25)*attribPrice for each purchase
		type: Gun, Helmet, Armor, Boots, Orb
		primary (75%): Health, Health Regen, Attack, Attack Speed, Jump Power, Move Speed
		secondary (25%): Uses list above
		strength:
			0:2500 Feeble,Flimsy,Fragile,Delicate,Weak,Minor
			2501:7000 Average,Fair,Familiar,Regular,Common,Ordinary
			7001:15000 Impressive,Powerful,Durable,Lasting,Firm,Reliable
			15001:25000 Massive,Sturdy,Unyielding,Mighty,Rugged,Enduring
	Author: Brian J. Glenn
	Date: 2014-11-14
*/
public class Item : MonoBehaviour
{
	public GameObject loot;
	public string itemName;
	public string[] description;
	public Attributes stats = new Attributes(); // actual stats on this item
	public int type = -1; // defines the icon/type of item, -1 means NULL
	public int power; // items power level
	public int rarity; // 0 thru 2
	public int orbA = 0; // 0 no orb slot, 1 empty orb slot, 2 filled orb slot
	public int orbB = 0;
	static int seedValue = 0;
	private int next(int current, int min, int max)
	{
		int ret = current;
		ret++;
		if(ret > max)
			ret = min;
		return ret;
	}
	private static string[,] descRarities = new string[,] // [3,6] [rarity,random]
		{{"Average ","Fair ","Familiar ","Regular ","Common ","Ordinary "},
		{"Impressive ","Strong ","Durable ","Lasting ","Firm ","Reliable "},
		{"Massive ","Timeless ","Unyielding ","Mighty ","Elite ","Enduring "}};
	private static string[,] descTypes = new string[,] // [5,3] [type, random]
		{{"Pistol of ","Blaster of ","Cannon of "},
		{"Hat of ","Headgear of ","Helmet of "},
		{"Vest of ","Jacket of ","Armor of "},
		{"Shoes of ","Boots of ","Footware of "},
		{"Orb of ","Crystal of ","Generator of "}};
	private static string[] descMainStats = new string[] // one subtype for each stat
		{"Vitality","Spirit","Power","Bombardment","Agility","Speed"};
/*
	generateLoot:
		level: Enter the level of item you wish to generate, you will receive an item of either
			green, blue, or purple rarity. Green is 100% standard weapon power, blue is 115%, and
			purple is 130% as strong.
		type: Pass a random number from 0 to 4.
		primary: Pass a random number from 0 to 5. Primary stat is 75% of the stats for the item.
		Secondary: Pass a random number from 0 to 5. Secondary stat is 25% of the stats for the item.
		Blue Items: Get an additional bonus stat worth 15%
		Purple Items: Get two additional bonus stats worth 15% each
*/
	public void generateLoot(int level, int type, int primary, int secondary, Transform trans)
	{
		int pri = primary;
		int sec = secondary;
		float[] statList = new float[] {0.0f,0.0f,0.0f,0.0f,0.0f,0.0f};
		float[] statCost = new float[] {1.0f,0.001f,3.0f,0.003f,1.0f,0.01f};
		bool[,] priRes = new bool[,] // health,regen,power,atkspd,jump,move
			{{false,false,true,true,false,false}, // weapon
			{true,true,true,true,false,false}, // head
			{true,true,true,true,false,false}, // chest
			{true,true,true,true,true,true}, // feet
			{true,true,true,true,true,true}}; // orbs
		bool[,] secRes = new bool[,] // health,regen,power,atkspd,jump,move
			{{true,true,true,true,false,false}, // weapon
			{true,true,true,true,false,false}, // head
			{true,true,true,true,true,true}, // chest
			{true,true,true,true,true,true}, // feet
			{true,true,true,true,true,true}}; // orbs
		int roll = Random.Range(900, 1000);//0, 1000
		Debug.Log (roll); //DELETE ME
		if(roll >= 600 && roll < 850) // drop small health
			loot = (GameObject)Instantiate(Resources.Load("Prefabs/smallHealth"));
		if(roll >= 850 && roll < 900) // drop large health
			loot = (GameObject)Instantiate(Resources.Load("Prefabs/largeHealth"));
		if(roll >= 900) // drop green
		{
			loot = (GameObject)Instantiate(Resources.Load("Prefabs/LootRarity"));
			loot.name += string.Format("{0:0}", seedValue);
			loot.transform.GetChild(0).name += string.Format("{0:0}", seedValue);
			seedValue++;
			SpriteRenderer lootRaritySprite = loot.GetComponent<SpriteRenderer>();
			SpriteRenderer lootSprite = loot.transform.GetChild (0).GetComponent<SpriteRenderer>();
			lootRaritySprite.sprite = (Sprite)Resources.Load ("Sprites/LootItemRarity", typeof(Sprite));
			lootRaritySprite.color = new Color(0.0f, 255.0f, 0.0f);
			while(priRes[type,pri] == false)
				pri = next(pri,0,5);
			while(secRes[type,sec] == false)
				sec = next(sec,0,5);
			this.type = type;
			loot.GetComponent<Item>().type = type;
			switch(type)
			{
			case 0: lootSprite.sprite = (Sprite)Resources.Load ("Sprites/gunSprite", typeof(Sprite));
				break;
			case 1: lootSprite.sprite = (Sprite)Resources.Load ("Sprites/helmSprite", typeof(Sprite));
				break;
			case 2: lootSprite.sprite = (Sprite)Resources.Load ("Sprites/chestSprite", typeof(Sprite));
				break;
			case 3: lootSprite.sprite = (Sprite)Resources.Load ("Sprites/bootsSprite", typeof(Sprite));
				break;
			case 4 : lootSprite.sprite = (Sprite)Resources.Load ("Sprites/laserGreen08", typeof(Sprite));
				break;
			}
			this.power = level;
			loot.GetComponent<Item>().power = level;
			this.rarity = 0;
			loot.GetComponent<Item>().rarity = 0;
			statList[pri] += level*statCost[pri]*0.75f;
			statList[sec] += level*statCost[sec]*0.25f;
			if(roll >= 980) // make blue
			{
				lootRaritySprite.sprite = (Sprite)Resources.Load ("Sprites/LootItemRarity", typeof(Sprite));
				lootRaritySprite.color = new Color(0.0f, 0.0f, 255.0f);
				statList[pri] += level*statCost[pri]*0.15f*0.75f;
				statList[sec] += level*statCost[sec]*0.15f*0.25f;
				this.rarity = 1;
				loot.GetComponent<Item>().rarity = 1;
				if(type != 4)// if not an orb, add empty orb socket
				{
					orbA = 1;
					loot.GetComponent<Item>().orbA = 1;
				}
			}
			if(roll >= 995) // make purple
			{
				lootRaritySprite.sprite = (Sprite)Resources.Load ("Sprites/LootItemRarity", typeof(Sprite));
				lootRaritySprite.color = new Color(255.0f, 0.0f, 255.0f);
				statList[pri] += level*statCost[pri]*0.15f*0.75f;
				statList[sec] += level*statCost[sec]*0.15f*0.25f;
				this.rarity = 2;
				loot.GetComponent<Item>().rarity = 2;
				if(type != 4)// if not an orb, add empty orb socket
				{
					orbB = 1;
					loot.GetComponent<Item>().orbB = 1;
				}
			}
			if(type == 4) // if an orb, reduce stats
				for(int i=0; i<6; i++)
					statList[i] *= 0.25f;
			if(statList[0] > 0.0f)
				statList[0] += statCost[0]*2.0f;
			if(statList[1] > 0.0f)
				statList[1] += statCost[1]*10.0f;
			if(statList[2] > 0.0f)
				statList[2] += statCost[2]*2.0f;
			if(statList[3] > 0.0f)
				statList[3] += statCost[3]*10.0f;
			if(statList[4] > 0.0f)
				statList[4] += statCost[4]*2.0f;
			if(statList[5] > 0.0f)
				statList[5] += statCost[5]*10.0f;
			loot.transform.GetChild(0).GetComponent<Attributes>().health = statList[0];
			loot.transform.GetChild(0).GetComponent<Attributes>().healthRegen = statList[1];
			loot.transform.GetChild(0).GetComponent<Attributes>().attack = statList[2];
			loot.transform.GetChild(0).GetComponent<Attributes>().attackSpeed = statList[3];
			loot.transform.GetChild(0).GetComponent<Attributes>().jumpHeight = statList[4];
			loot.transform.GetChild(0).GetComponent<Attributes>().moveSpeed = statList[5];
			stats.health = statList[0];
			stats.healthRegen = statList[1];
			stats.attack = statList[2];
			stats.attackSpeed = statList[3];
			stats.jumpHeight = statList[4];
			stats.moveSpeed = statList[5];
			itemName = descRarities[rarity,Random.Range(0,5)] + descTypes[type,Random.Range(0,2)] +
				descMainStats[pri];
			loot.GetComponent<Item>().itemName = itemName;
			generateEffectList();
		}
		if(roll >= 600)
			loot.transform.position = trans.position;
		if(roll >= 900)
		{
			Debug.Log ("Loot Name: " + itemName);
			for(int i = 0; i < description.Length; i++)
			{
				Debug.Log ("Log Description: " + description[i]);
			}
		}

	}
	private int totalVariations(float test)
	{
		int count = 0;
		if(stats.health != test) count++;
		if(stats.healthRegen != test) count++;
		if(stats.attack != test) count++;
		if(stats.attackSpeed != test) count++;
		if(stats.jumpHeight != test) count++;
		if(stats.moveSpeed != test) count++;
		return count;
	}
	private void generateEffectList()
	{
		float testVal = 0.0f;
		int total = totalVariations(testVal);
		if(total != 0)
		{
			description = new string[total];
			int pos = 0;
			if(stats.health != testVal)
			{
				description[pos] = string.Format("{0:0}",stats.health) + " Health";
				pos++;
			}
			if(stats.healthRegen != testVal)
			{
				description[pos] = string.Format("{0:0}",stats.healthRegen * 100.0f) + "% Health Regen";
				pos++;
			}
			if(stats.attack != testVal)
			{
				description[pos] = string.Format("{0:0}",stats.attack) + " Weapon Power";
				pos++;
			}
			if(stats.attackSpeed != testVal)
			{
				description[pos] = string.Format("{0:0}",stats.attackSpeed * 100.0f) + "% Weapon Speed";
				pos++;
			}
			if(stats.jumpHeight != testVal)
			{
				description[pos] = string.Format("{0:0}",stats.jumpHeight) + " Jump";
				pos++;
			}
			if(stats.moveSpeed != testVal)
			{
				description[pos] = string.Format("{0:0}",stats.moveSpeed * 10.0f) + "% Movement";
				pos++;
			}

			loot.GetComponent<Item>().description = description;
		}
		else
			description = new string[] {"Null Item"};
	}
}
