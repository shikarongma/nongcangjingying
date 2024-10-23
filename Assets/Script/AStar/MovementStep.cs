using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.AStar
{
    //移动的每一步步骤
    public class MovementStep
    {
        public string sceneName;//这一步所在场景
        public int hour;/*需要的时间*/
        public int minute;
        public int second;
        public Vector2Int gridCoordinate;//这里的坐标是实际场景的坐标
    }
}

