using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropDetails cropDetails;
    private int harvestActionCount;
    public TileDetails tileDetails;

    public bool CanHarvest => cropDetails.TotalGrowthDays <= tileDetails.growthDays;

    private Animator anim;

    private Transform PlayerTransfrom => FindObjectOfType<PlayerControl>().transform;
    public void ProcessToolAction(ItemDetails tool, TileDetails tile)
    {
        tileDetails = tile;
        //工具使用次数
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;

        anim = GetComponentInChildren<Animator>();

        //点击计数器
        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;
            //判断是否有动画 树木
            if (anim != null && cropDetails.hasAnimation)
            {
                if (PlayerTransfrom.position.x < transform.position.x)
                    anim.SetTrigger("RotateRight");
                else
                    anim.SetTrigger("RotateLeft");
            }
            //播放粒子
            //播放声音
        }

        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generatePlayerPosiotion || !cropDetails.hasAnimation)
            {
                //生成农作物
                SpawnHarvestItems();
            }
            else if (cropDetails.hasAnimation)
            {
                if (PlayerTransfrom.position.x > transform.position.x)
                {
                    anim.SetTrigger("FallingLeft");
                }
                else
                    anim.SetTrigger("FallingRight");
                StartCoroutine(HarvestAfterAnimation());
            }
        }


    }

    private IEnumerator HarvestAfterAnimation()
    {
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("END"))
        {
            yield return null;
        }

        SpawnHarvestItems();
        //转换新物体
        if (cropDetails.transferItemID > 0)
        {
            CreateTransferCrop();
        }
    }

    private void CreateTransferCrop()
    {
        tileDetails.seedItemID = cropDetails.transferItemID;
        tileDetails.daysSinceLastHarvest = -1;
        tileDetails.growthDays = 0;

        EventHandler.CallRefreshCurrentMap();

    }

    public void SpawnHarvestItems()
    {
        for(int i = 0; i < cropDetails.produceItemID.Length; i++)
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
            for(int j = 0; j < amountToProduce; j++)
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


        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;

            //是否可以重复生长
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //刷新种子
                EventHandler.CallRefreshCurrentMap();
            }
            else//不可重复生长
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
            }

            Destroy(gameObject);
        }
    }
}
