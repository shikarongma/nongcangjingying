using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "SO/Map/MapData_SO")]
public class MapData_SO : ScriptableObject
{
    //��������
    public string sceneName;
    //������ӵ�����Ե���Ƭ
    public List<TileProperty> tileProperties;
}
