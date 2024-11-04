using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public const float itemDuration = 0.35f;//���뽥��ʱ��
    public const float targetAlpha = 0.45f;//͸����

    //ʱ��������� ϵͳ�趨ʱ��
    public const float secondThreshold = 0.01f;//��ֵԽСʱ��Խ��, Ҳ����ʵ����Ϸʱ�������һ��
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //�������뽥����ʱ��
    public const float fadeDuration = 1.5f;

    //�����������
    public const int reapAmount = 2;

    //NPC�����ƶ�
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;//б�������
    public const float pixelSize = 0.05f;//���ؾ���
    public const float animationBreakTime = 5f;//�������

    public const int maxGridSize = 9999;

}
