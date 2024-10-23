using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "SO/Map/MapData_SO")]
public class MapData_SO : ScriptableObject
{
    //��������
    public string sceneName;
    [Header("��ͼ��Ϣ")]
    public int gridWidth;
    public int gridHeight;
    [Header("���½�ԭ��")]
    public int originX;
    public int originY;
    //������ӵ�����Ե���Ƭ
    public List<TileProperty> tileProperties;
}
