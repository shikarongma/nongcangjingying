using MFram.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MFram.InvenTory
{
    public class ItemManager : MonoBehaviour
    {
        //ֱ�����ɵ���ƷԤ���壨����Ӱ�ģ�
        public Item itemPerfab;
        //��������϶���������ƷԤ���壨����Ӱ�ģ�
        public Item bounceItemPerfab;

        private Transform itemParent;//��������ĸ�����

        private Transform PlayerTransform => FindObjectOfType<PlayerControl>().transform;

        //�����ֵ䣬��¼�����е�item
        private Dictionary<string, List<SceneItem>> sceneItemDic = new Dictionary<string, List<SceneItem>>();

        //�����¼�������Ʒ�Ϸŵ�������
        private void OnEnable()
        {
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//ֱ��������Ʒ
            EventHandler.DropItemEvent += OnDropItemEvent;//����Ӱ��������Ʒ
        }

        private void OnDisable()
        {
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        //Ѱ����Ʒ�ĸ����壨���
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
            //mousePos������Ʒ���յ�λ�ã�������ʱ���ɵ�λ��Ӧ��ΪPlayer��λ�ã���Ϊ�Ǵ�player�������ɳ�����
            var item = Instantiate(bounceItemPerfab, PlayerTransform.position, Quaternion.identity, itemParent);
            item.itemID = itemID;
            var dir = (mousepPos - PlayerTransform.position).normalized;//normalized,����������Ʒ�ɳ��ķ���
            item.GetComponent<ItemBounce>().InitBounceItem(mousepPos, dir);
        }

        /// <summary>
        /// ��ȡ��ǰ�����е�ȫ��item
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
                //����ֵ��Ѿ��������������������������item
                sceneItemDic[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else
            {
                //���������³�����ֱ�����
                sceneItemDic.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        /// <summary>
        /// ˢ�����õ�ǰ������Ʒ
        /// </summary>
        private void RecreateAllItem()
        {
            List<SceneItem> currentSceneItem = new List<SceneItem>();

            //TryGetValue�Ǵ��ֵ���ֵ�����ҵ��ʹ洢��currentSceneItem�У�û���򷵻�Ĭ��ֵ
            if (sceneItemDic.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItem))
            {
                if (currentSceneItem != null)
                {
                    //�峡
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

