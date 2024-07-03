using MFram.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MFram.InvenTory
{
    public class ItemManager : MonoBehaviour
    {
        //直接生成的物品预制体（无阴影的）
        public Item itemPerfab;
        //从玩家手上丢出来的物品预制体（有阴影的）
        public Item bounceItemPerfab;

        private Transform itemParent;//生成物体的父物体

        private Transform PlayerTransform => FindObjectOfType<PlayerControl>().transform;

        //创建字典，记录场景中的item
        private Dictionary<string, List<SceneItem>> sceneItemDic = new Dictionary<string, List<SceneItem>>();

        //接受事件：将物品拖放到地面上
        private void OnEnable()
        {
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//直接生成物品
            EventHandler.DropItemEvent += OnDropItemEvent;//有阴影的生成物品
        }

        private void OnDisable()
        {
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        //寻找物品的父物体（载物）
        private void OnAfterSceneUnloadEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItem();
        }

        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItem();
        }

        private void OnInstantiateItemInScene(int itemID, Vector3 position)
        {
            var item = Instantiate(itemPerfab, position, Quaternion.identity, itemParent);
            item.itemID = itemID;
        }

        private void OnDropItemEvent(int itemID, Vector3 mousepPos)
        {
            //mousePos（即物品最终的位置），而此时生成的位置应该为Player的位置，因为是从player身上生成出来的
            var item = Instantiate(bounceItemPerfab, PlayerTransform.position, Quaternion.identity, itemParent);
            item.itemID = itemID;
            var dir = (mousepPos - PlayerTransform.position).normalized;//normalized,向量化，物品飞出的方向
            item.GetComponent<ItemBounce>().InitBounceItem(mousepPos, dir);
        }

        /// <summary>
        /// 获取当前场景中的全部item
        /// </summary>
        private void GetAllSceneItem()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach(var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position=new SerialiazbleVector3(item.transform.position)
                };

                currentSceneItems.Add(sceneItem);
            }

            if (sceneItemDic.ContainsKey(SceneManager.GetActiveScene().name)){
                //如果字典已经有这个场景，则更新这个场景的item
                sceneItemDic[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else
            {
                //否则则是新场景，直接添加
                sceneItemDic.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        /// <summary>
        /// 刷新重置当前场景物品
        /// </summary>
        private void RecreateAllItem()
        {
            List<SceneItem> currentSceneItem = new List<SceneItem>();

            //TryGetValue是从字典找值，若找到就存储在currentSceneItem中，没有则返回默认值
            if (sceneItemDic.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItem))
            {
                if (currentSceneItem != null)
                {
                    //清场
                    foreach(var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach(var item in currentSceneItem)
                    {
                        Item newItem = Instantiate(itemPerfab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
    }
}

