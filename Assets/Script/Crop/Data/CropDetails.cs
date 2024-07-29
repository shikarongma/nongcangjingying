using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    public int seedItemID;
    [Header("不同阶段需要的天数")]
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

    [Header("不同生长阶段物品perfab")]
    public GameObject[] growthPrefabs;
    [Header("不同阶段的图片")]
    public Sprite[] growthSprites;
    [Header("可种植的季节")]
    public Season[] seasons;

    [Space]
    [Header("收割工具")]
    public int[] harvestToolItemID;
    [Header("每种工具使用次数")]
    public int[] requireActionCount;
    [Header("转换新物品ID")]
    public int transferItemID;

    [Space]
    [Header("收割果实信息")]
    public int[] produceItemID;
    public int[] produceMinAmount;
    public int[] produceMaxAmount;
    public Vector2 spawnRadius;//生成果实的范围

    [Header("再次生长时间")]
    public int daysToRegrow;//再次生长的时间
    public int regrowTimes;//重生次数

    [Header("Options")]//选项
    public bool generatePlayerPosiotion;//是否生成在人物身上
    public bool hasAnimation;//是否有动画
    public bool hasParticalEffect;//是否有粒子效果

    //TODO:特效，音效等

    /// <summary>
    /// 判断是否可以用这个工具收割该农作物
    /// </summary>
    /// <param name="toolID">工具ID</param>
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
    /// 获取工具使用次数
    /// </summary>
    /// <param name="toolID">工具ID</param>
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
