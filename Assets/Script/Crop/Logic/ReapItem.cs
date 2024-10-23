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
        /// 生成果实
        /// </summary>
        public void SpawnHarvestItems()
        {
            for (int i = 0; i < cropDetails.produceItemID.Length; i++)
            {
                int amountToProduce;

                if (cropDetails.produceMinAmount[i] == cropDetails.produceMaxAmount[i])
                {
                    //代表只生成固定数量的
                    amountToProduce = cropDetails.produceMinAmount[i];
                }
                else
                {
                    //物品数量随机
                    amountToProduce = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
                }

                //执行生成物品
                for (int j = 0; j < amountToProduce; j++)
                {
                    if (cropDetails.generatePlayerPosiotion)
                        EventHandler.CallHarvestAtPlayerPosition(cropDetails.produceItemID[i]);
                    else
                    {
                        //世界地图上生成,比如树木之类的
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

