using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AfterSceneUnloadEvent += SwitchConfinerShape;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneUnloadEvent -= SwitchConfinerShape;
    }

    //private void Start()
    //{
    //    SwitchConfinerShape();
    //}
    //寻找相机边框
    private void SwitchConfinerShape()
    {
        //寻找bounds，获取其身上的组件
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
        //获取自身组件
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        //赋值
        confiner.m_BoundingShape2D = confinerShape;

        confiner.InvalidatePathCache();
    }
}
