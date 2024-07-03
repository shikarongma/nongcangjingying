using MFram.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MFrom.Inventory
{
    public class ShowItenTolltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private SlotUI slotUI;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemDetails != null)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.SetUpToolTip(slotUI.itemDetails, slotUI.slotType);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;
                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }
    }
}

