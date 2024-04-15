using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFram.Inventory{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null){
                InventroyManager.Instance.AddItem(item, true);
            }
        }
    }
}

