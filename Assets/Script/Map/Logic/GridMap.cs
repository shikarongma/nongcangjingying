using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

//�ڱ༭ģʽ������
[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    //��Ƭ����
    public GridType gridType;
    //��Ƭ���ݣ�scriptObject��
    public MapData_SO mapData;
    //��ǰ��tileMap
    private Tilemap currentTilemap;

    //ע��Ϊʲô������Onenable��Ondisable����Ϊ����������������ͼ�������رյ������ִ��
    private void OnEnable()
    {
        //�жϵ�ǰ�Ƿ��ڵ������gameobject
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            //���¸���
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

            //�ڱ༭ģʽ������
#if UNITY_EDITOR
            if (mapData != null)
            {
                EditorUtility.SetDirty(mapData);
            }
#endif
        }
    }

    //������Ƭ
    private void UpdateTileProperties()
    {
        currentTilemap.CompressBounds();

        if (!Application.IsPlaying(this))
        {
            if (mapData != null)
            {
                //�ѻ��Ʒ�Χ�����½�����
                Vector3Int startPos = currentTilemap.cellBounds.min;
                //�ѻ��Ʒ�Χ�����Ͻ�����
                Vector3Int endPos = currentTilemap.cellBounds.max;

                //����ʼ�����α���
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
