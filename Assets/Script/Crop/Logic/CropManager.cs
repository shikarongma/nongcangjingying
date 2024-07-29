using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.CropPlant
{
    public class CropManager : Singleton<CropManager>
    {
        //种子数据库
        public CropDataList_SO cropData;

        private Grid currentGrid;
        private Transform currentParent;
        private Season currentSeason;

        private void OnEnable()
        {
            EventHandler.PlantSeedEvent += OnPlantSeedEvent;
            EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
            EventHandler.GameDayEvent += OnGameDayEvent;
        }

        private void OnDisable()
        {
            EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
            EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
            EventHandler.GameDayEvent -= OnGameDayEvent;
        }

        private void OnPlantSeedEvent(int itemID, TileDetails tileDetails)
        {
            CropDetails currentCrop = GetCropDetails(itemID);
            if (currentCrop != null && SeasonAvailable(currentCrop) && tileDetails.seedItemID == -1)
            {
                tileDetails.seedItemID = itemID;
                tileDetails.growthDays = 0;
                //显示农作物
                DisplayCropPlant(tileDetails, currentCrop);
            }else if (tileDetails.seedItemID != -1)
            {
                //显示农作物
                DisplayCropPlant(tileDetails, currentCrop);
            }
        }

        private void OnAfterSceneUnloadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            currentParent = GameObject.FindWithTag("CropParent").transform;
        }

        private void OnGameDayEvent(int day, Season season)
        {
            currentSeason = season;
        }

        /// <summary>
        /// 显示农作物
        /// </summary>
        /// <param name="tileDetails">瓦片地图信息</param>
        /// <param name="cropDetails">种子信息</param>
        private void DisplayCropPlant(TileDetails tileDetails,CropDetails cropDetails)
        {
            //成长阶段
            int growthStages = cropDetails.growthDays.Length;
            int currentStage = 0;//当前生长阶段
            int dayCounter = cropDetails.TotalGrowthDays;

            //倒序计算当前的成长阶段
            for(int i = growthStages - 1; i >= 0; i--)
            {
                if (tileDetails.growthDays >= dayCounter)
                {
                    currentStage = i;
                    break;
                }
                dayCounter -= cropDetails.growthDays[i];
            }

            //获取当前的prefab
            GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];
            Sprite cropSprite = cropDetails.growthSprites[currentStage];

            Vector3 pos = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);

            GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, currentParent);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;

            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
            cropInstance.GetComponent<Crop>().tileDetails = tileDetails;
        }

        /// <summary>
        /// 根据种子ID获取种子信息
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public CropDetails GetCropDetails(int itemID)
        {
            //for(int i = 0; i < cropData.cropDetailsList.Count; i++)
            //{
            //    if (itemID == cropData.cropDetailsList[i].seedItemID)
            //        return cropData.cropDetailsList[i];
            //}
            //return null;
            return cropData.cropDetailsList.Find(c => c.seedItemID == itemID);
        }

        /// <summary>
        /// 判断当前季节是否可以种植
        /// </summary>
        /// <param name="crop"></param>
        /// <returns></returns>
        private bool SeasonAvailable(CropDetails crop)
        {
            for(int i = 0; i < crop.seasons.Length; i++)
            {
                if (crop.seasons[i] == currentSeason)
                    return true;
            }
            return false;
        }
    }
}
