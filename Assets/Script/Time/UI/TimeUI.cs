using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage; //日月交换图片
    public RectTransform clockParent;//六个时间块的父物体
    public TextMeshProUGUI dateText;//日期
    public TextMeshProUGUI timeText;//时间
    public Image seasonImage;//季节图片

    public Sprite[] seasonSprite;//存储各个季节图片

    private List<GameObject> clockBlocks = new List<GameObject>();//时间块

    private void Awake()
    {
        for(int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockBlocks[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }

    private void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }

    //时间的改变
    private void OnGameMinuteEvent(int hour, int minute)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    //日期、日月改变，时间块改变
    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        dateText.text = year + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";
        seasonImage.sprite = seasonSprite[(int)season];
        DayNightImageRotate(hour);
        SwitchHourIamge(hour);
    }

    //日月改变
    private void DayNightImageRotate(int hour)
    {
        var target = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);
    }

    //时间块改变
    private void SwitchHourIamge(int hour)
    {
        int index = hour / 4;
        if (index == 0)
        {
            foreach(var item in clockBlocks)
            {
                item.SetActive(false);
            }
        }
        else
        {
            for(int i = 0; i < clockBlocks.Count; i++)
            {
                if (i < index + 1)
                {
                    clockBlocks[i].SetActive(true);
                }
                else
                {
                    clockBlocks[i].SetActive(false);
                }
            }
        }
    }
}
