public enum ItemType
{

    Seed,Commodity,Furniture,//种子，商品，家具
    //锄头，斧头，破碎工具，收割工具，浇水，收集蓝
    HoeTool,ChopTool,BreakTool,ReapTool,WaterTool,CollectTool,
    ReapableScenery//杂草
}

//槽型
public enum SlotType
{
    Bag,Box,Shop
}


//物品所在位置
public enum InventoryLocation
{
    //玩家背包，场景箱子
    Player,Box
}

//物品的使用
public enum PartType
{
    None,Carry,Hoe,Break,Water
}
//玩家部分
public enum PartName
{
    Body,Hair,Arm,Tool
}
//季节
public enum Season
{
    春季,夏季,秋季,冬季
}

//瓦片的类型
public enum GridType
{
    //挖，丢物品， ，npc障碍
    Diggable,DropItem,PlaceFurniture,NpcObstacle
}