using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	public GameObject MenuPanel;
	public GameObject Player;
	public GameObject InventoryMenu;
	public GameObject InventoryContentsPanel;
	public GameObject DetailsPanel;
	public GameObject EquippedPanel;
	public GameObject EquippedDetailsPanel;
	public LootManager lootManager;

	PlayerCharacter playerController;


	// Use this for initialization
	void Start () 
	{
		MenuPanel.SetActive (false);
		InventoryMenu.SetActive (false);
		lootManager = GameObject.Find("LootManager").GetComponent("LootManager") as LootManager;
		playerController = Player.GetComponent<PlayerCharacter> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void MenuButtonPressed()
	{

		if(MenuPanel.activeSelf)
		{
			MenuPanel.SetActive(false);
			Time.timeScale = 1.0f;
		}
		else
		{
			MenuPanel.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}

	public void activateInventoryMenu()
	{
		InventoryMenu.SetActive (true);
		MenuPanel.SetActive (false);

		//Load the inventory
		for(int i=0; i<lootManager.inventoryTotal; i++)
		{
			InventoryContentsPanel.transform.GetChild(i).GetComponentInChildren<Text>().text = 
				lootManager.inventory[i].GetComponent<Item>().itemName;
				
			InventoryContentsPanel.transform.GetChild(i).Find("RarityImage").GetComponent<Image>().sprite =
				lootManager.inventory[i].GetComponent<SpriteRenderer>().sprite;

			InventoryContentsPanel.transform.GetChild(i).Find("RarityImage").GetComponent<Image>().color =
				lootManager.inventory[i].GetComponent<SpriteRenderer>().color;

			InventoryContentsPanel.transform.GetChild(i).Find("RarityImage").GetChild(0).GetComponent<Image>().sprite =
				lootManager.inventory[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

			InventoryContentsPanel.transform.GetChild(i).Find("RarityImage").GetChild(0).GetComponent<Image>().color =
				lootManager.inventory[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;

			InventoryContentsPanel.transform.GetChild(i).Find("ItemLevelText").GetComponent<Text>().text = "Level " + 
				lootManager.inventory[i].GetComponent<Item>().power;
		}

		for(int i = 0; i < lootManager.equipped.Length; i++)
		{
			EquippedPanel.transform.GetChild(i).Find("ItemNameText").GetComponent<Text>().text = 
				lootManager.equipped[i].GetComponent<Item>().itemName;

			EquippedPanel.transform.GetChild(i).Find("ItemLevelText").GetComponent<Text>().text = "Level " + 
				lootManager.equipped[i].GetComponent<Item>().power.ToString();

			EquippedPanel.transform.GetChild(i).Find("RarityImage").GetComponent<Image>().sprite = 
				lootManager.equipped[i].GetComponent<SpriteRenderer>().sprite;

			EquippedPanel.transform.GetChild(i).Find("RarityImage").GetComponent<Image>().color = 
				lootManager.equipped[i].GetComponent<SpriteRenderer>().color;

			EquippedPanel.transform.GetChild(i).Find("RarityImage").GetChild(0).GetComponent<Image>().sprite = 
				lootManager.equipped[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

			EquippedPanel.transform.GetChild(i).Find("RarityImage").GetChild(0).GetComponent<Image>().color = 
				lootManager.equipped[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color;
		}

	}

	public void deactivateInventoryMenu()
	{
		InventoryMenu.SetActive (false);
		MenuPanel.SetActive (true);

		DetailsPanel.transform.Find ("DescriptionText").GetComponent<Text> ().text = null;
		DetailsPanel.transform.Find ("ItemNameText").GetComponent<Text> ().text = null;
		DetailsPanel.transform.Find ("RarityImage").GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		DetailsPanel.transform.Find ("RarityImage").GetChild (0).GetComponent<Image> ().color = new Color (0, 0, 0, 0);
		DetailsPanel.transform.Find ("ItemLevelText").GetComponent<Text> ().text = null;
	}


	public void inventoryItemPressed(int ind)
	{
		Debug.Log (lootManager.inventory[ind].description.Length);

		string temp = "";

		for(int i = 0; i < lootManager.inventory[ind].description.Length; i++)
		{
			temp += lootManager.inventory[ind].description[i] + "\n";
		}

		Debug.Log (temp);

		DetailsPanel.transform.Find ("DescriptionText").GetComponent<Text> ().text = temp;

		DetailsPanel.transform.Find ("ItemNameText").GetComponent<Text> ().text = lootManager.inventory [ind].itemName;

		DetailsPanel.transform.Find ("RarityImage").GetComponent<Image> ().color = 
			lootManager.inventory [ind].GetComponent<SpriteRenderer> ().color;

		DetailsPanel.transform.Find ("RarityImage").GetChild (0).GetComponent<Image> ().sprite = 
			lootManager.inventory [ind].transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite;

		DetailsPanel.transform.Find ("RarityImage").GetChild (0).GetComponent<Image> ().color = 
			lootManager.inventory [ind].transform.GetChild (0).GetComponent<SpriteRenderer> ().color;

		DetailsPanel.transform.Find ("ItemLevelText").GetComponent<Text> ().text = "Level " + 
			lootManager.inventory [ind].power;

	}

	public void equippedItemPressed(int ind)
	{
		string temp = "";
		
		for(int i = 0; i < lootManager.equipped[ind].description.Length; i++)
		{
			temp += lootManager.equipped[ind].description[i] + "\n";
		}
		
		Debug.Log (temp);
		
		EquippedDetailsPanel.transform.Find ("DescriptionText").GetComponent<Text> ().text = temp;
		
		EquippedDetailsPanel.transform.Find ("ItemNameText").GetComponent<Text> ().text = lootManager.equipped [ind].itemName;
		
		EquippedDetailsPanel.transform.Find ("RarityImage").GetComponent<Image> ().color = 
			lootManager.equipped [ind].GetComponent<SpriteRenderer> ().color;
		
		EquippedDetailsPanel.transform.Find ("RarityImage").GetChild (0).GetComponent<Image> ().sprite = 
			lootManager.equipped [ind].transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite;
		
		EquippedDetailsPanel.transform.Find ("RarityImage").GetChild (0).GetComponent<Image> ().color = 
			lootManager.equipped [ind].transform.GetChild (0).GetComponent<SpriteRenderer> ().color;
		
		EquippedDetailsPanel.transform.Find ("ItemLevelText").GetComponent<Text> ().text = "Level " + 
			lootManager.equipped [ind].power;
	}

}
