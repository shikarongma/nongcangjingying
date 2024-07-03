using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace MFarm.Map
{
    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("�ֵ���Ƭ�л���Ϣ")]
        public RuleTile digTile;
        public RuleTile waterTile;
        private Tilemap digTileMap;
        private Tilemap waterTileMap;


        private Grid currentGrid;
        private Season currentSeason;

        [Header("��ͼ��Ϣ")]
        public List<MapData_SO> mapDataList;

        //��Ƭ�ֵ䣬�ؼ���Ϊ����+����name��keyΪ��Ƭ
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

        //��GridMapManager��ʵ����Ʒ�����������Ϊ��Ʒ�ܶ඼��ı�Grid
        private void OnEnable()
        {
            EventHandler.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
        }

        private void OnDisable()
        {
            EventHandler.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            EventHandler.GameDayEvent-= OnGameDayEvent;
        }
 

        private void Start()
        {
            foreach(var mapData in mapDataList)
            {
                InitTileDetailsDict(mapData);
            }
        }

        private void OnAfterSceneUnloadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            digTileMap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterTileMap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();

            RefresMap();
        }

        /// <summary>
        /// ������µ�ͼ
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="season"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnGameDayEvent(int gameDay, Season season)
        {
            currentSeason = season;
            foreach(var tile in tileDetailsDict)
            {
                if (tile.Value.waterDays > -1)
                {
                    tile.Value.waterDays = -1;
                }
                if (tile.Value.digDays > -1)
                {
                    tile.Value.digDays++;
                }
                //���������ڿ�
                if (tile.Value.digDays > 5 && tile.Value.seedItemID == -1)
                {
                    tile.Value.digDays = -1;
                    tile.Value.canDig = true;
                }

                RefresMap();
            }
        }

        /// <summary>
        /// ���ݵ�ͼ������Ƭ����
        /// </summary>
        /// <param name="mapData"></param>
        private void InitTileDetailsDict(MapData_SO mapData)
        {
            foreach(var tileProperty in mapData.tileProperties)
            {
                string key = tileProperty.tileCoordinate.x + "x" + tileProperty.tileCoordinate.y + "y" + mapData.sceneName;
                TileDetails tileDetails = new TileDetails();
                if (GetTileDetails(key) != null){
                    tileDetails = tileDetailsDict[key];
                }
                else
                {
                    tileDetails.gridX = tileProperty.tileCoordinate.x;
                    tileDetails.gridY = tileProperty.tileCoordinate.y;
                }

                switch (tileProperty.gridType)
                {
                    case GridType.Diggable:
                        tileDetails.canDig = tileProperty.boolTypeValue;
                        break;
                    case GridType.DropItem:
                        tileDetails.canDropItem = tileProperty.boolTypeValue;
                        break;
                    case GridType.PlaceFurniture:
                        tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                        break;
                    case GridType.NpcObstacle:
                        tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key) != null)
                {
                    tileDetailsDict[key] = tileDetails;
                }
                else
                    tileDetailsDict.Add(key, tileDetails);
            }

        }

        /// <summary>
        /// ������Ƭλ���Լ���ͼ���Ʒ�����Ƭ����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private TileDetails GetTileDetails(string key)
        {
            if (tileDetailsDict.ContainsKey(key))
            {
                return tileDetailsDict[key];
            }
            else
                return null;
        }

        /// <summary>
        /// ��������������귵����Ƭ��Ϣ
        /// </summary>
        /// <param name="mouseGridPos"></param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        /// <summary>
        /// ���������Ӧ��Ʒʹ�ö�����ʵ����Ʒʹ�õ�Ч��
        /// </summary>
        /// <param name="mouseWorldPos"></param>
        /// <param name="itemDetails"></param>
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            //��ǰ������ڵ���������
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                //TODO:�������͵���Ʒ����ʹ�ù��ܻ�δ��д
                //��Ʒʹ��ʵ�ʹ���
                switch (itemDetails.itemType)
                {
                    case ItemType.Commodity:
                        EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos);
                        break;
                    case ItemType.HoeTool:
                        SetDigGround(currentTile);
                        currentTile.digDays = 0;
                        currentTile.canDig = false;
                        currentTile.canDropItem = false;
                        //��Ч
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.waterDays = 0;
                        //��Ч
                        break;
                }

                UpdateTileDetails(currentTile);
            }
            
        }

        /// <summary>
        /// ��ʾ�ڿ���Ƭ
        /// </summary>
        /// <param name="tile"></param>
        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if(digTileMap!=null)
                digTileMap.SetTile(pos, digTile);
        }
        
        /// <summary>
        /// ��ʾ��ˮ��Ƭ
        /// </summary>
        /// <param name="tile"></param>
        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if(waterTileMap!=null)
                waterTileMap.SetTile(pos, waterTile);
        }

        /// <summary>
        /// ������Ƭ��Ϣ
        /// </summary>
        /// <param name="tileDetails"></param>
        private void UpdateTileDetails(TileDetails tileDetails)
        {
            string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
            if (tileDetailsDict.ContainsKey(key))
            {
                tileDetailsDict[key] = tileDetails;
            }
        }

        /// <summary>
        /// ˢ�µ�ǰ��ͼ
        /// </summary>
        private void RefresMap()
        {
            if (digTileMap != null)
            {
                digTileMap.ClearAllTiles();
            }
            if (waterTileMap != null)
            {
                waterTileMap.ClearAllTiles();
            }

            DisplayMap(SceneManager.GetActiveScene().name);
        }


        /// <summary>
        /// ��ʾ��ͼ��Ƭ
        /// </summary>
        /// <param name="sceneName"></param>
         private void DisplayMap(string sceneName)
        {
            foreach(var value in tileDetailsDict)
            {
                var key = value.Key;
                var tileDetail = value.Value;

                if (key.Contains(sceneName))
                {
                    if (tileDetail.digDays > -1)
                        SetDigGround(tileDetail);
                    if (tileDetail.waterDays > -1)
                        SetWaterGround(tileDetail);

                }
            }
        }

    }
}

