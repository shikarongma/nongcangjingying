using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "SO/Map/MapData_SO")]
public class MapData_SO : ScriptableObject
{
    //场景名字
    public string sceneName;
    [Header("地图信息")]
    public int gridWidth;
    public int gridHeight;
    [Header("左下角原点")]
    public int originX;
    public int originY;
    //场景中拥有属性的瓦片
    public List<TileProperty> tileProperties;
}
