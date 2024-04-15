using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Inventory/InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
}
