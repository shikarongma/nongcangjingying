using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    //����NPC���ƶ�
    public List<NPCPosition> NpcPositionList;
    //�糡�����ƶ�
    public SceneRouteDataList_SO SceneRouteData;

    private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

    private void Start()
    {
        InitSceneRouteDict();
    }
    /// <summary>
    /// ��ʼ��·���ֵ䣨�糡����
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
    /// ��ȡ�����������·��
    /// </summary>
    /// <param name="formSceneName">��ʼ����</param>
    /// <param name="gotoSceneName">Ŀ�곡��</param>
    /// <returns></returns>
    public SceneRoute GetSceneRoute(string fromSceneName,string gotoSceneName)
    {
        return sceneRouteDict[fromSceneName + gotoSceneName];
    }
}
