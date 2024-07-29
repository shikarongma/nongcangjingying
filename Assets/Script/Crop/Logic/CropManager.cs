using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.CropPlant
{
    public class CropManager : Singleton<CropManager>
    {
        //�������ݿ�
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
                //��ʾũ����
                DisplayCropPlant(tileDetails, currentCrop);
            }else if (tileDetails.seedItemID != -1)
            {
                //��ʾũ����
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
        /// ��ʾũ����
        /// </summary>
        /// <param name="tileDetails">��Ƭ��ͼ��Ϣ</param>
        /// <param name="cropDetails">������Ϣ</param>
        private void DisplayCropPlant(TileDetails tileDetails,CropDetails cropDetails)
        {
            //�ɳ��׶�
            int growthStages = cropDetails.growthDays.Length;
            int currentStage = 0;//��ǰ�����׶�
            int dayCounter = cropDetails.TotalGrowthDays;

            //������㵱ǰ�ĳɳ��׶�
            for(int i = growthStages - 1; i >= 0; i--)
            {
                if (tileDetails.growthDays >= dayCounter)
                {
                    currentStage = i;
                    break;
                }
                dayCounter -= cropDetails.growthDays[i];
            }

            //��ȡ��ǰ��prefab
            GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];
            Sprite cropSprite = cropDetails.growthSprites[currentStage];

            Vector3 pos = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);

            GameObject cropInstance = Instantiate(cropPrefab, pos, Quaternion.identity, currentParent);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;

            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
            cropInstance.GetComponent<Crop>().tileDetails = tileDetails;
        }

        /// <summary>
        /// ��������ID��ȡ������Ϣ
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
        /// �жϵ�ǰ�����Ƿ������ֲ
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
