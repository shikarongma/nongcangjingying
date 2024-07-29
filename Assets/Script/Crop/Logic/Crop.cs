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
        //����ʹ�ô���
        int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
        if (requireActionCount == -1) return;

        anim = GetComponentInChildren<Animator>();

        //���������
        if (harvestActionCount < requireActionCount)
        {
            harvestActionCount++;
            //�ж��Ƿ��ж��� ��ľ
            if (anim != null && cropDetails.hasAnimation)
            {
                if (PlayerTransfrom.position.x < transform.position.x)
                    anim.SetTrigger("RotateRight");
                else
                    anim.SetTrigger("RotateLeft");
            }
            //��������
            //��������
        }

        if (harvestActionCount >= requireActionCount)
        {
            if (cropDetails.generatePlayerPosiotion || !cropDetails.hasAnimation)
            {
                //����ũ����
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
        //ת��������
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
                //����ֻ���ɹ̶�������
                amountToProduce = cropDetails.produceMinAmount[i];
            }
            else
            {
                //��Ʒ�������
                amountToProduce = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
            }

            //ִ��������Ʒ
            for(int j = 0; j < amountToProduce; j++)
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


        if (tileDetails != null)
        {
            tileDetails.daysSinceLastHarvest++;

            //�Ƿ�����ظ�����
            if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes)
            {
                tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                //ˢ������
                EventHandler.CallRefreshCurrentMap();
            }
            else//�����ظ�����
            {
                tileDetails.daysSinceLastHarvest = -1;
                tileDetails.seedItemID = -1;
            }

            Destroy(gameObject);
        }
    }
}
