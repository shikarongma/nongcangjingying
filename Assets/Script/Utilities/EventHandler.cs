using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

//事件中心
public static class EventHandler
{

    /// <summary>
    /// 物品添加的UI中
    /// </summary>
    public static event Action<InventoryLocation,List<InventoryItem>> UpdateInVentoryUI;
    public static void CallUpdateInventoryUI(InventoryLocation location,List<InventoryItem> list)
    {
        UpdateInVentoryUI.Invoke(location, list);
    }

    /// <summary>
    /// 直接生成物品
    /// </summary>
    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int itemID,Vector3 position)
    {
        InstantiateItemInScene?.Invoke(itemID, position);
    }

    /// <summary>
    /// 从玩家手中丢出去的时候生成的物品（会实现物品飞出效果）
    /// </summary>
    public static event Action<int, Vector3, ItemType> DropItemEvent;
    public static void CallDropItemEvent(int itemID,Vector3 position, ItemType itemType)
    {
        DropItemEvent?.Invoke(itemID, position, itemType);
    }

    /// <summary>
    /// 点击物品高亮
    /// </summary>
    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    /// <summary>
    /// 更改时间UI
    /// </summary>
    public static event Action<int, int, int, Season> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute, int hour, int day, Season season)
    {
        GameMinuteEvent?.Invoke(minute, hour, day, season);
    }

    public static event Action<int, Season> GameDayEvent;
    public static void CallGameDayEvent(int day, Season season)
    {
        GameDayEvent?.Invoke(day, season);
    }

    /// <summary>
    /// 更改时间UI
    /// </summary>
    public static event Action<int, int, int, int, Season> GameDateEvent;
    public static void CallGameDateEvent(int hour,int day,int month,int year,Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    public static event Action<String, Vector3> TransitionEvent;
    public static void CallTransition(String sceneName,Vector3 position)
    {
        TransitionEvent?.Invoke(sceneName, position);
    }

    /// <summary>
    /// 切换场景前需要做的事
    /// </summary>
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    /// <summary>
    /// 切换场景后需要做的事
    /// </summary>
    public static event Action AfterSceneUnloadEvent;
    public static void CallAfterSceneUnloadEvent()
    {
        AfterSceneUnloadEvent?.Invoke();
    }

    /// <summary>
    /// 更改玩家位置
    /// </summary>
    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 nextPosition)
    {
        MoveToPosition?.Invoke(nextPosition);
    }

    /// <summary>
    /// 玩家点击背包物品后，鼠标点击完成事件
    /// </summary>
    public static event Action<Vector3, ItemDetails> MouseClickedEvent;
    public static void CallMouseClickedEvent(Vector3 mouseWorldPos,ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(mouseWorldPos, itemDetails);
    }

    /// <summary>
    /// Player做完动画后，执行具体事件
    /// </summary>
    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;
    public static void CallExecuteActionAfterAnimation(Vector3 mouseWorldPos,ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimation?.Invoke(mouseWorldPos, itemDetails);
    }

    public static event Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int itemID,TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(itemID, tileDetails);
    }

    /// <summary>
    /// 农作物成熟收集
    /// </summary>
    public static event Action<int> HarvestAtPlayerPosition;
    public static void CallHarvestAtPlayerPosition(int itemID)
    {
        HarvestAtPlayerPosition?.Invoke(itemID);
    }

    /// <summary>
    /// 刷新地图
    /// </summary>
    public static event Action RefreshCurrentMap;
    public static void CallRefreshCurrentMap()
    {
        RefreshCurrentMap?.Invoke();
    }

    /// <summary>
    /// 粒子对象
    /// </summary>
    public static event Action<ParticleEffectType, Vector3> ParticleEffectEvent;
    public static void CallParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
    {
        ParticleEffectEvent?.Invoke(effectType, pos);
    }

    public static event Action GenerateCropEvent;
    public static void CallGenerateCropEvent()
    {
        GenerateCropEvent?.Invoke();
    }
}
