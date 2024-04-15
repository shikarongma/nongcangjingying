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
    /// 将物品丢在地上
    /// </summary>
    public static event Action<int, Vector3> InstantiateItemInScene;

    public static void CallInstantiateItemInScene(int itemID,Vector3 position)
    {
        InstantiateItemInScene?.Invoke(itemID, position);
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
    public static event Action<int, int> GameMinuteEvent;

    public static void CallGameMinuteEvent(int hour,int minute)
    {
        GameMinuteEvent?.Invoke(hour, minute);
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
}
