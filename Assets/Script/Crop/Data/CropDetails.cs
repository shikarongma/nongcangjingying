using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("��ͬ�׶���Ҫ������")]
    public int[] growthDays;
    public int TotalGrowthDays
    {
        get
        {
            int amount = 0;
            foreach(var days in growthDays)
            {
                amount += days;
            }
            return amount;
        }
    }

    [Header("��ͬ�����׶���Ʒperfab")]
    public GameObject[] growthPrefabs;
    [Header("��ͬ�׶ε�ͼƬ")]
    public Sprite[] growthSprites;
    [Header("����ֲ�ļ���")]
    public Season[] seasons;

    [Space]
    [Header("�ո��")]
    public int[] harvestToolItemID;
    [Header("ÿ�ֹ���ʹ�ô���")]
    public int[] requireActionCount;
    [Header("ת������ƷID")]
    public int transferItemID;

    [Space]
    [Header("�ո��ʵ��Ϣ")]
    public int[] produceItemID;
    public int[] produceMinAmount;
    public int[] produceMaxAmount;
    public Vector2 spawnRadius;//���ɹ�ʵ�ķ�Χ

    [Header("�ٴ�����ʱ��")]
    public int daysToRegrow;//�ٴ�������ʱ��
    public int regrowTimes;//��������

    [Header("Options")]//ѡ��
    public bool generatePlayerPosiotion;//�Ƿ���������������
    public bool hasAnimation;//�Ƿ��ж���
    public bool hasParticalEffect;//�Ƿ�������Ч��

    //TODO:��Ч����Ч��

    /// <summary>
    /// �ж��Ƿ��������������ո��ũ����
    /// </summary>
    /// <param name="toolID">����ID</param>
    /// <returns></returns>
    public bool CheckToolAvailable(int toolID)
    {
        foreach(var tool in harvestToolItemID)
        {
            if(tool==toolID)
                return true;
        }
        return false;
    }

    /// <summary>
    /// ��ȡ����ʹ�ô���
    /// </summary>
    /// <param name="toolID">����ID</param>
    /// <returns></returns>
    public int GetTotalRequireCount(int toolID)
    {
        for(int i = 0; i < harvestToolItemID.Length; i++)
        {
            if (harvestToolItemID[i] == toolID)
                return requireActionCount[i];
        }
        return -1;
    }
}