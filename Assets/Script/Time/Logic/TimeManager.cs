using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //秒分时 日月年
    private int gameSecond, gameMinute, gameHour, gamedDay, gameMonth, gameYear;

    //四个月为一个季节
    private int monthInSeason = 3;

    //季节
    private Season gameSeason;

    //暂停时间
    public bool gameClockPause;

    //当前时间
    private float currentTime;

    private void Awake()
    {
        NewGameTime();
    }

    private void Start()
    {
        EventHandler.CallGameMinuteEvent(gameHour, gameMinute);
        EventHandler.CallGameDateEvent(gameHour, gamedDay, gameMonth, gameYear, gameSeason);
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= Settings.secondThreshold)
            {
                currentTime = 0;
                UpdateGameTime();
            }
        }
        //测试地图挖坑...
        if (Input.GetKeyDown(KeyCode.G))
        {
            gamedDay++;
            EventHandler.CallGameDayEvent(gamedDay, gameSeason);
            EventHandler.CallGameDateEvent(gameHour, gamedDay, gameMonth, gameYear, gameSeason);

        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;//从早上七点开始
        gamedDay = 25;
        gameMonth = 3;
        gameYear = 2024;
        gameSeason = Season.春季;
    }

    private void UpdateGameTime()
    {
        gameSecond++;
        if (gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;
            if (gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;
                if (gameHour > Settings.hourHold)
                {
                    gamedDay++;
                    gameHour = 0;
                    if (gamedDay > Settings.dayHold)
                    {
                        gameMonth++;
                        gamedDay = 1;

                        if (gameMonth > 12)
                        {
                            gameMonth = 1;
                        }
                        monthInSeason--;
                        //说明过完一个季节了
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if (seasonNumber > Settings.seasonHold)//说明一年四季都过了
                            {
                                seasonNumber = 0;//过第二年春季
                                gameYear++;
                            }
                            gameSeason = (Season)seasonNumber;

                            if (gameYear > 9999)
                            {
                                gameYear = 2024;
                            }
                        }
                        //每过一天刷新地图和农作物生长
                        EventHandler.CallGameDayEvent(gamedDay, gameSeason);
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gamedDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameHour, gameMinute);
        }
        //Debug.Log(gameSecond + " " + gameMinute);
    }
}
