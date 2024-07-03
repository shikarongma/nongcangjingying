using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

//若要使用item相关类，需先调用其命名空间
namespace MFram.Inventory
{
    public class InventroyManager : Singleton<InventroyManager>
    {
        //获得物品详情
        [Header("物品数据")]
        public ItemDataList_SO itemdataList_SO;

        //获得背包详情
        [Header("背包数据")]
        public InventoryBag_SO playerBag;

        private void OnEnable()
        {//从玩家手里物品丢出来生成物品（在这里主要是实现背包物品数量的更改）
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
        }

        //将物品拖到地上
        private void OnDropItemEvent(int itemID, Vector3 position)
        {
            RemoveItem(itemID, 1);
        }

        //初始化背包UI里面的物品
        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        //通过ID返回物品信息
        public ItemDetails GetItemDetails(int ID)
        {
            return itemdataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        //添加物品到玩家的背包
        public void AddItem(Item item,bool toDestory)
        {
            //判断背包中是否有该物品
            int index = GetItemIndexInBag(item.itemID);

            //添加物品进背包
            AddItemInBag(item.itemID, index, 1);

            
            Debug.Log(GetItemDetails(item.itemID).itemID + GetItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }
            //通过事件将物品添加的UI中
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
        /// <summary>
        /// 检查背包是否有空位
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
        /// 通过物品ID判断背包里是否有该物品，并返回物品序号
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
        /// 添加物品进背包
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        private void AddItemInBag(int itemID,int index,int count)
        {
            if (index != -1)//背包中有该物品
            {
                int currentAmount = playerBag.itemList[index].itemAmount + count;
                InventoryItem newItem = new InventoryItem { itemID = itemID, itemAmount = currentAmount };

                playerBag.itemList[index] = newItem;
            }
            else if (CheckBagCapacity()) //背包中没有该物体
            {
                InventoryItem newItem = new InventoryItem { itemID = itemID, itemAmount = count };
                //寻找位置为空
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
                Debug.Log("背包已满");
        }

        /// <summary>
        /// Player背包内交换物品
        /// </summary>
        /// <param name="fromIndex">交换的物品序号</param>
        /// <param name="targetIndex">被交换的物品序号</param>

        //拖拽物品交换位置
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
            //刷新背包UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 移除指定背包物品
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
            //呼叫背包UI，更新
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}
