using MFrom.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MFram.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("��קͼƬ")]
        public Image dragImage;

        //���slotUI
        public SlotUI[] playerSlot;

        //���BagUI
        [SerializeField] private GameObject BagUI;

        //���ItemToolTip
        public ItemToolTip itemToolTip;

        private bool isOpen;


        private void Start()
        {
            isOpen = BagUI.activeInHierarchy;

            for(int i = 0; i < playerSlot.Length; i++)
            {
                playerSlot[i].index = i;
            }
        }

        //�������Ʊ���Ui�Ĵ�
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnEnable()
        {
            //����Ʒ���뱳��UI��
            EventHandler.UpdateInVentoryUI += UpdataInVentorySlotUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInVentoryUI -= UpdataInVentorySlotUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        }

        private void UpdataInVentorySlotUI(InventoryLocation location, List<InventoryItem> itemList)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    //�ҵ�˼·��������������Ʒ
                    //int index = 0;
                    //for (int i = 0; i < itemList.Count; i++)
                    //{
                    //    if (itemList[i].itemID == 0)
                    //        continue;
                    //    var item = InventroyManager.Instance.GetItemDetails(itemList[i].itemID);
                    //    slots[index].UpdataSlot(item, itemList[i].itemAmount);
                    //    index++;
                    //}

                    //��ڵ�˼·����������������ɶ����UIҲ��ɶ������λ����
                    for (int i = 0; i < playerSlot.Length; i++)
                    {
                        if (itemList[i].itemID == 0)
                            playerSlot[i].UpdateEmptySlot();
                        else
                        {
                            var item = InventroyManager.Instance.GetItemDetails(itemList[i].itemID);
                            playerSlot[i].UpdataSlot(item, itemList[i].itemAmount);
                        }
                    }
                    break;
            }
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHighlight(-1);
        }

        public void OpenBagUI()
        {
            isOpen = !isOpen;
            BagUI.SetActive(isOpen);
        }

        /// <summary>
        /// ����slot������ʾ
        /// </summary>
        /// <param name = "index" > itemID </ param >
        public void UpdateSlotHighlight(int index)
        {
            foreach (var slot in playerSlot)
            {
                if (slot.index == index && slot.isSelected)
                {
                    slot.highLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.highLight.gameObject.SetActive(false);
                }
            }
        }
    }
}

