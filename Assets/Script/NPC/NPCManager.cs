using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    //所有NPC的移动
    public List<NPCPosition> NpcPositionList;
    //跨场景的移动
    public SceneRouteDataList_SO SceneRouteData;

    private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

    private void Start()
    {
        InitSceneRouteDict();
    }
    /// <summary>
    /// 初始化路径字典（跨场景）
    /// </summary>
    private void InitSceneRouteDict()
    {
        if (SceneRouteData.sceneRoutes.Count > 0)
        {
            foreach(SceneRoute route in SceneRouteData.sceneRoutes)
            {
                string key = route.fromSceneName + route.gotoSceneName;
                if (sceneRouteDict.ContainsKey(key))
                    continue;
                else
                    sceneRouteDict.Add(key, route);
            }
        }
        foreach(var route in sceneRouteDict.Keys)
        {
            Debug.Log(route);
        }
    }

    /// <summary>
    /// 获取两个场景间的路径
    /// </summary>
    /// <param name="formSceneName">起始场景</param>
    /// <param name="gotoSceneName">目标场景</param>
    /// <returns></returns>
    public SceneRoute GetSceneRoute(string fromSceneName,string gotoSceneName)
    {
        return sceneRouteDict[fromSceneName + gotoSceneName];
    }
}
