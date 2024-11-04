using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public const float itemDuration = 0.35f;//渐入渐出时间
    public const float targetAlpha = 0.45f;//透明度

    //时间相关数据 系统设定时间
    public const float secondThreshold = 0.01f;//数值越小时间越快, 也就是实际游戏时间里面的一秒
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //场景渐入渐出的时间
    public const float fadeDuration = 1.5f;

    //割草数量限制
    public const int reapAmount = 2;

    //NPC网格移动
    public const float gridCellSize = 1;
    public const float gridCellDiagonalSize = 1.41f;//斜方向距离
    public const float pixelSize = 0.05f;//像素距离
    public const float animationBreakTime = 5f;//动画间隔

    public const int maxGridSize = 9999;

}
