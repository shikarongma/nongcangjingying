using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

//在编辑模式下运行
[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    //瓦片类型
    public GridType gridType;
    //瓦片数据（scriptObject）
    public MapData_SO mapData;
    //当前的tileMap
    private Tilemap currentTilemap;

    //注：为什么是引用Onenable和Ondisable，因为这个代码需在这个地图启动、关闭的情况下执行
    private void OnEnable()
    {
        //判断当前是否在调用这个gameobject
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            //重新更新
            if (mapData != null)
                mapData.tileProperties.Clear();
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            UpdateTileProperties();

            //在编辑模式下运行
#if UNITY_EDITOR
            if (mapData != null)
            {
                EditorUtility.SetDirty(mapData);
            }
#endif
        }
    }

    //更新瓦片
    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();

        if (!Application.IsPlaying(this))
        {
            if (mapData != null)
            {
                //已绘制范围的左下角坐标
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //已绘制范围的右上角坐标
                Vector3Int endPos = currentTilemap.cellBounds.max;

                //从起始点依次遍历
                for(int x = startPos.x; x < endPos.x; x++)
                {
                    for(int y = startPos.y; y < endPos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            TileProperty newTile = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                gridType = this.gridType,
                                boolTypeValue=true
                            };
                            mapData.tileProperties.Add(newTile);

                        }
                    }
                }
            }
        }
    }
}
