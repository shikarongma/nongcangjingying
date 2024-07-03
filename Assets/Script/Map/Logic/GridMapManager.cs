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
        [Header("种地瓦片切换信息")]
        public RuleTile digTile;
        public RuleTile waterTile;
        private Tilemap digTileMap;
        private Tilemap waterTileMap;


        private Grid currentGrid;
        private Season currentSeason;

        [Header("地图信息")]
        public List<MapData_SO> mapDataList;

        //瓦片字典，关键字为坐标+场景name，key为瓦片
        private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

        //在GridMapManager里实现物品具体操作，因为物品很多都会改变Grid
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
        /// 按天更新地图
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
                //超期消除挖坑
                if (tile.Value.digDays > 5 && tile.Value.seedItemID == -1)
                {
                    tile.Value.digDays = -1;
                    tile.Value.canDig = true;
                }

                RefresMap();
            }
        }

        /// <summary>
        /// 根据地图更新瓦片详情
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
        /// 根据瓦片位置以及地图名称返回瓦片属性
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
        /// 根据鼠标网格坐标返回瓦片信息
        /// </summary>
        /// <param name="mouseGridPos"></param>
        /// <returns></returns>
        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
        {
            string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        /// <summary>
        /// 玩家做完相应物品使用动画后，实际物品使用的效果
        /// </summary>
        /// <param name="mouseWorldPos"></param>
        /// <param name="itemDetails"></param>
        private void OnExecuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            //当前鼠标所在的网格坐标
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

            if (currentTile != null)
            {
                //TODO:其他类型的物品具体使用功能还未书写
                //物品使用实际功能
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
                        //音效
                        break;
                    case ItemType.WaterTool:
                        SetWaterGround(currentTile);
                        currentTile.waterDays = 0;
                        //音效
                        break;
                }

                UpdateTileDetails(currentTile);
            }
            
        }

        /// <summary>
        /// 显示挖坑瓦片
        /// </summary>
        /// <param name="tile"></param>
        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if(digTileMap!=null)
                digTileMap.SetTile(pos, digTile);
        }
        
        /// <summary>
        /// 显示浇水瓦片
        /// </summary>
        /// <param name="tile"></param>
        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridX, tile.gridY, 0);
            if(waterTileMap!=null)
                waterTileMap.SetTile(pos, waterTile);
        }

        /// <summary>
        /// 更新瓦片信息
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
        /// 刷新当前地图
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
        /// 显示地图瓦片
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

