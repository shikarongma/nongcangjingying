using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleDataList_SO", menuName = "SO/Schedule/ScheduleDataList_SO")]
public class ScheduleDataList_SO : ScriptableObject
{
    public List<ScheduleDetails> scheduleList;
}
