using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

//��Ҫʹ��item����࣬���ȵ����������ռ�
namespace MFram.Inventory
{
    public class InventroyManager : Singleton<InventroyManager>
    {
        //�����Ʒ����
        [Header("��Ʒ����")]
        public ItemDataList_SO itemdataList_SO;

        //��ñ�������
        [Header("��������")]
        public InventoryBag_SO playerBag;

        private void OnEnable()
        {//�����������Ʒ������������Ʒ����������Ҫ��ʵ�ֱ�����Ʒ�����ĸ��ģ�
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
        }

        //����Ʒ�ϵ�����
        private void OnDropItemEvent(int itemID, Vector3 position)
        {
            RemoveItem(itemID, 1);
        }

        //��ʼ������UI�������Ʒ
        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        //ͨ��ID������Ʒ��Ϣ
        public ItemDetails GetItemDetails(int ID)
        {
            return itemdataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        //�����Ʒ����ҵı���
        public void AddItem(Item item,bool toDestory)
        {
            //�жϱ������Ƿ��и���Ʒ
            int index = GetItemIndexInBag(item.itemID);

            //�����Ʒ������
            AddItemInBag(item.itemID, index, 1);

            
            Debug.Log(GetItemDetails(item.itemID).itemID + GetItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }
            //ͨ���¼�����Ʒ��ӵ�UI��
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
        /// <summary>
        /// ��鱳���Ƿ��п�λ
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// ͨ����ƷID�жϱ������Ƿ��и���Ʒ����������Ʒ���
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        private int GetItemIndexInBag(int itemID)
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == itemID)
                {
                    return i;
                }
            }
            return - 1;
        }
        
        /// <summary>
        /// �����Ʒ������
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        private void AddItemInBag(int itemID,int index,int count)
        {
            if (index != -1)//�������и���Ʒ
            {
                int currentAmount = playerBag.itemList[index].itemAmount + count;
                InventoryItem newItem = new InventoryItem { itemID = itemID, itemAmount = currentAmount };

                playerBag.itemList[index] = newItem;
            }
            else if (CheckBagCapacity()) //������û�и�����
            {
                InventoryItem newItem = new InventoryItem { itemID = itemID, itemAmount = count };
                //Ѱ��λ��Ϊ��
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = newItem;
                        break;
                    }
                }
            }
            else
                Debug.Log("��������");
        }

        /// <summary>
        /// Player�����ڽ�����Ʒ
        /// </summary>
        /// <param name="fromIndex">��������Ʒ���</param>
        /// <param name="targetIndex">����������Ʒ���</param>

        //��ק��Ʒ����λ��
        public void SwapItem(int fromIndex,int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[fromIndex];
            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if (targetItem.itemID > 0)
            {
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetIndex] = currentItem;
            }
            else
            {
                playerBag.itemList[fromIndex] = new InventoryItem();
                playerBag.itemList[targetIndex] = currentItem;
            }
            //ˢ�±���UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// �Ƴ�ָ��������Ʒ
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="removeAmount"></param>
        public void RemoveItem(int ID, int removeAmount)
        {
            int index = GetItemIndexInBag(ID);

            if (playerBag.itemList[index].itemAmount > removeAmount)
            {
                int currentAmount = playerBag.itemList[index].itemAmount - removeAmount;
                InventoryItem item = new InventoryItem
                {
                    itemID = ID,
                    itemAmount = currentAmount
                };

                playerBag.itemList[index] = item;
            }
            else if(playerBag.itemList[index].itemAmount == removeAmount)
            {
                InventoryItem item = new InventoryItem
                {
                    itemID = 0,
                    itemAmount = 0
                };
                playerBag.itemList[index] = item;
            }
            //���б���UI������
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}
