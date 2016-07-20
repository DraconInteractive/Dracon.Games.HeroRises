using UnityEngine;
using System.Collections;
[CreateAssetMenu(fileName="Item", menuName = "Inventory/Item", order = 1)]
public class InventoryObject : ScriptableObject {

	public string itemName;
	public int itemQuantity;

	public enum itemTypes {SAB, TWOH, DW, GOLD, GEM, AR};
	public itemTypes type;

	public int stage, cost, itemID;

	public int dps, armour;

	public void SaveItemDetails(){
		PlayerPrefs.SetInt(itemName + "Quantity", itemQuantity);
	}

	public void LoadItemVariables(){
		itemQuantity = PlayerPrefs.GetInt(itemName + "Quantity");
	}

	public void ResetItemVariables(){
		itemQuantity = 0;
	}
}
