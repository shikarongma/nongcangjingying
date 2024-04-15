using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MFrom.Inventory
{
    public class ItemToolTip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI discription;
        [SerializeField] private Text valueText;
        [SerializeField] private GameObject button;

        public void SetUpToolTip(ItemDetails item,SlotType slotType)
        {
            nameText.text = item.itemName;
            typeText.text = GetItemType(item.itemType);
            discription.text = item.itemDescription;
            if (item.itemType == ItemType.Seed || item.itemType == ItemType.Commodity || item.itemType == ItemType.Furniture)
            {
                var price = item.itemPrice;
                if (slotType == SlotType.Bag)
                {
                    price = (int)(price * item.sellPercentage);
                }
                valueText.text = price.ToString();
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }
            //�����ֶ���ʱ��ǿ��������ת��
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        private string GetItemType(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Seed => "����",
                ItemType.Commodity => "��Ʒ",
                ItemType.Furniture => "ˮ��",
                ItemType.HoeTool => "����",
                ItemType.ChopTool => "����",
                ItemType.BreakTool => "����",
                ItemType.ReapTool => "����",
                ItemType.WaterTool => "����",
                ItemType.CollectTool=> "����",
                _ =>"��"
            };
        }
    }
}

