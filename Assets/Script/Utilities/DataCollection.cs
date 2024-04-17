using UnityEngine;

//物品属性
[System.Serializable]
public class ItemDetails
{
    public ItemType itemType;

    public int itemID;

    public string itemName;

    [Header("物品图标")]
    public Sprite itemIcon;

    [Header("物品在地上的图标")]
    public Sprite itemOnWorldSprite;

    public string itemDescription;

    [Header("物品占地面积")]
    public int itemUseRadius;

    [Header("物品是否可拾取")]
    public bool canPickedup;

    [Header("物品是否可以扔掉")]
    public bool canDropped;

    [Header("物品是否可举起来")]
    public bool canCarried;

    public int itemPrice;

    [Header("销售百分比")]
    [Range(0, 1)]
    public float sellPercentage;
}

//展示的物品
[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

//动画类型
[System.Serializable]
public struct AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

//场景中各个item的位置
[System.Serializable]
public class SerialiazbleVector3
{
    public float x, y, z;

    public SerialiazbleVector3(Vector3 position)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

//场景的item
[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerialiazbleVector3 position;
}

//瓦片的属性
[System.Serializable]
public class TileProperty
{
    //坐标
    public Vector2Int tileCoordinate;
    //类型
    public GridType gridType;
    //
    public bool boolTypeValue;
}