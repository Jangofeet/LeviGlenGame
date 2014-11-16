using UnityEngine;
using System.Collections;

public class LootManager : MonoBehaviour
{
	public Item[] equipped = new Item[4];
	public Item[] inventory = new Item[50];
	public int inventoryTotal = 0;
	private Item temp = new Item();


	void Start()
	{
		for(int x = 0; x < inventory.Length; x++)
		{
			Item blankItem = new Item();
			blankItem.type = -1;
			inventory[x] = blankItem;
		}
	}


	// places looted item in inventory
	public void lootItem(Item lootedItem)
	{
		if(findFirstEmpty() != -1)
		{
			inventory[findFirstEmpty()] = lootedItem;
			inventoryTotal++;
		}
	}

	// equips item in inventoryLocation, places old eqiupped item in that spot
	public void equipItem(int inventoryLocation)
	{
		temp = equipped[inventory[inventoryLocation].type];
		equipped[inventory[inventoryLocation].type] = inventory[inventoryLocation];
		inventory[inventoryLocation] = temp;
		if(inventory[inventoryLocation].type == -1)
			inventoryTotal--;
	}
	// sorts inventory so actual items are at front of array, sorts by type, blank items at end
	// sorts in order of guns, head, body, boots, orbs, empty
	public void sortInventory()
	{
		bool sorted = false;
		if(findLastItem() != 0)
			while(sorted == false)
			{
				sorted = true;
				for(int i=1; i<=findLastItem(); i++)
					if(inventory[i-1].type > inventory[i].type && inventory[i].type != -1)
					{
						swapInventoryItems(i,i-1);
						sorted = false;
					}
			}
	}
	public bool isInventoryFull()
	{
		if(inventoryTotal >= inventory.Length-1)
			return true;
		return false;
	}

	// returns first empty inventory slot, -1 if inventory is full
	private int findFirstEmpty()
	{
		for(int i=0; i<inventory.Length; i++)
			if(inventory[i].type == -1)
				return i;
		return -1;
	}
	// finds last item in inventory, returns -1 if no items in inventory
	private int findLastItem()
	{
		int lastFound = -1;
		for(int i=0; i<inventory.Length; i++)
			if(inventory[i].type != -1)
				lastFound = i;
		return lastFound;
	}
	private void swapInventoryItems(int src, int dst)
	{
		temp = inventory[dst];
		inventory[dst] = inventory[src];
		inventory[src] = temp;
	}
}
