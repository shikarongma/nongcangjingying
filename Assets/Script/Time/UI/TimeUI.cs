using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage; //���½���ͼƬ
    public RectTransform clockParent;//����ʱ���ĸ�����
    public TextMeshProUGUI dateText;//����
    public TextMeshProUGUI timeText;//ʱ��
    public Image seasonImage;//����ͼƬ

    public Sprite[] seasonSprite;//�洢��������ͼƬ

    private List<GameObject> clockBlocks = new List<GameObject>();//ʱ���

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

    //ʱ��ĸı�
    private void OnGameMinuteEvent(int hour, int minute)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    //���ڡ����¸ı䣬ʱ���ı�
    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        dateText.text = year + "��" + month.ToString("00") + "��" + day.ToString("00") + "��";
        seasonImage.sprite = seasonSprite[(int)season];
        DayNightImageRotate(hour);
        SwitchHourIamge(hour);
    }

    //���¸ı�
    private void DayNightImageRotate(int hour)
    {
        var target = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);
    }

    //ʱ���ı�
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
