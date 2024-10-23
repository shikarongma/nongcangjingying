using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.CropPlant
{
    public class ReapItem : MonoBehaviour
    {
        private CropDetails cropDetails;

        private Transform PlayerTransfrom => FindObjectOfType<PlayerControl>().transform;

        public void InitCropData(int itemID)
        {
            cropDetails = CropManager.Instance.GetCropDetails(itemID);
        }

        /// <summary>
        /// ���ɹ�ʵ
        /// </summary>
        public void SpawnHarvestItems()
        {
            for (int i = 0; i < cropDetails.produceItemID.Length; i++)
            {
                int amountToProduce;

                if (cropDetails.produceMinAmount[i] == cropDetails.produceMaxAmount[i])
                {
                    //����ֻ���ɹ̶�������
                    amountToProduce = cropDetails.produceMinAmount[i];
                }
                else
                {
                    //��Ʒ�������
                    amountToProduce = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
                }

                //ִ��������Ʒ
                for (int j = 0; j < amountToProduce; j++)
                {
                    if (cropDetails.generatePlayerPosiotion)
                        EventHandler.CallHarvestAtPlayerPosition(cropDetails.produceItemID[i]);
                    else
                    {
                        //�����ͼ������,������ľ֮���
                        var dirX = transform.position.x > PlayerTransfrom.position.x ? 1 : -1;

                        var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                        transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                        EventHandler.CallInstantiateItemInScene(cropDetails.produceItemID[i], spawnPos);
                    }
                }
            }

        }
    }
}

