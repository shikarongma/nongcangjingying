using MFram.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MFrom.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //物品图片
        [SerializeField] private Image slotImage;
        //物品个数
        [SerializeField] private TextMeshProUGUI amountText;
        //选择高亮
        public Image highLight;
        //获得Button的Interactable(不可选择)
        [SerializeField] private Button button;

        [Header("格子类型")]
        public SlotType slotType;
        //是否被点按
        public bool isSelected;

        //物品信息
        public ItemDetails itemDetails;
        private int itemAmount;

        //物品序号
        public int index;

        //获得InventoryUI
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        

        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)
            {
                UpdateEmptySlot();
            }
        }

        public void UpdataSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            itemAmount = amount;
            slotImage.sprite = item.itemIcon;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }


        /// <summary>
        /// 初始化Slot
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        //点击高亮
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails.itemID == 0)
            {
                return;
            }
            isSelected = !isSelected;
            inventoryUI.UpdateSlotHighlight(index);

            //呼叫事件，玩家举起物品
            if (slotType == SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        //开始拖拽
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragImage.enabled = true;
                inventoryUI.dragImage.sprite = slotImage.sprite;

                //拖拽显示高亮
                isSelected = true;
                inventoryUI.UpdateSlotHighlight(index);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //赋值位置
            inventoryUI.dragImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            inventoryUI.dragImage.enabled = false;

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //丢在BagUI里
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.index;

                //在背包里交换
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventroyManager.Instance.SwapItem(index, targetIndex);
                }

                //关闭高亮
                inventoryUI.UpdateSlotHighlight(-1);
            }
            //丢在地上
            else
            {
                if (itemDetails.canDropped)
                {
                    //鼠标对应的地图位置
                    var position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    //呼叫事件
                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, position);
                }
                
            }
        }
    }
}
