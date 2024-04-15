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
        //��ƷͼƬ
        [SerializeField] private Image slotImage;
        //��Ʒ����
        [SerializeField] private TextMeshProUGUI amountText;
        //ѡ�����
        public Image highLight;
        //���Button��Interactable(����ѡ��)
        [SerializeField] private Button button;

        [Header("��������")]
        public SlotType slotType;
        //�Ƿ񱻵㰴
        public bool isSelected;

        //��Ʒ��Ϣ
        public ItemDetails itemDetails;
        private int itemAmount;

        //��Ʒ���
        public int index;

        //���InventoryUI
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
        /// ��ʼ��Slot
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

        //�������
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails.itemID == 0)
            {
                return;
            }
            isSelected = !isSelected;
            inventoryUI.UpdateSlotHighlight(index);

            //�����¼�����Ҿ�����Ʒ
            if (slotType == SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        //��ʼ��ק
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)
            {
                inventoryUI.dragImage.enabled = true;
                inventoryUI.dragImage.sprite = slotImage.sprite;

                //��ק��ʾ����
                isSelected = true;
                inventoryUI.UpdateSlotHighlight(index);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //��ֵλ��
            inventoryUI.dragImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            inventoryUI.dragImage.enabled = false;

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //����BagUI��
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetSlot.index;

                //�ڱ����ｻ��
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
                {
                    InventroyManager.Instance.SwapItem(index, targetIndex);
                }

                //�رո���
                inventoryUI.UpdateSlotHighlight(-1);
            }
            //���ڵ���
            else
            {
                if (itemDetails.canDropped)
                {
                    //����Ӧ�ĵ�ͼλ��
                    var position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                    //�����¼�
                    EventHandler.CallInstantiateItemInScene(itemDetails.itemID, position);
                }
                
            }
        }
    }
}
