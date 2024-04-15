using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MFron.InvenTory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPerfab;

        private Transform itemParent;

        //创建字典，记录场景中的item
        private Dictionary<string, List<SceneItem>> sceneItemDic = new Dictionary<string, List<SceneItem>>();

        //接受事件：将物品拖放到地面上
        private void OnEnable()
        {
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
        }

        private void OnDisable()
        {
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
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

        //将物品拖到地上
        private void OnInstantiateItemInScene(int itemID, Vector3 position)
        {
            var item = Instantiate(itemPerfab, position, Quaternion.identity, itemParent);
            item.itemID = itemID;
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

