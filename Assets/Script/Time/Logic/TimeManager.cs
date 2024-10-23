using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    //���ʱ ������
    private int gameSecond, gameMinute, gameHour, gamedDay, gameMonth, gameYear;

    //�ĸ���Ϊһ������
    private int monthInSeason = 3;

    //����
    private Season gameSeason;

    //��ͣʱ��
    public bool gameClockPause;

    //��ǰʱ��
    private float currentTime;

    public TimeSpan GameTime => new TimeSpan(gameHour, gameMinute, gameSecond);

    protected override void Awake()
    {
        base.Awake();
        NewGameTime();
    }

    private void Start()
    {
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gamedDay, gameSeason);
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
        //���Ե�ͼ�ڿ�...
        if (Input.GetKeyDown(KeyCode.G))
        {
            gameMinute++;
            EventHandler.CallGameDayEvent(gamedDay, gameSeason);
            EventHandler.CallGameDateEvent(gameHour, gamedDay, gameMonth, gameYear, gameSeason);

        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;//�������ߵ㿪ʼ
        gamedDay = 25;
        gameMonth = 3;
        gameYear = 2024;
        gameSeason = Season.����;
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
                        //˵������һ��������
                        if (monthInSeason == 0)
                        {
                            monthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if (seasonNumber > Settings.seasonHold)//˵��һ���ļ�������
                            {
                                seasonNumber = 0;//���ڶ��괺��
                                gameYear++;
                            }
                            gameSeason = (Season)seasonNumber;

                            if (gameYear > 9999)
                            {
                                gameYear = 2024;
                            }
                        }
                        //ÿ��һ��ˢ�µ�ͼ��ũ��������
                        EventHandler.CallGameDayEvent(gamedDay, gameSeason);
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gamedDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour, gamedDay, gameSeason);
        }
        //Debug.Log(gameSecond + " " + gameMinute);
    }
}
