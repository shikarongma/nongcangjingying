using MFrom.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MFram.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("拖拽图片")]
        public Image dragImage;

        //获得slotUI
        public SlotUI[] playerSlot;

        //获得BagUI
        [SerializeField] private GameObject BagUI;

        //获得ItemToolTip
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

        //按键控制背包Ui的打开
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnEnable()
        {
            //将物品放入背包UI中
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
                    //我的思路，挨个挨个放物品
                    //int index = 0;
                    //for (int i = 0; i < itemList.Count; i++)
                    //{
                    //    if (itemList[i].itemID == 0)
                    //        continue;
                    //    var item = InventroyManager.Instance.GetItemDetails(itemList[i].itemID);
                    //    slots[index].UpdataSlot(item, itemList[i].itemAmount);
                    //    index++;
                    //}

                    //麦口的思路，背包里面数据是啥样，UI也是啥样，空位保持
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
        /// 更新slot高亮显示
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

