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
    //Ѱ������߿�
    private void SwitchConfinerShape()
    {
        //Ѱ��bounds����ȡ�����ϵ����
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
        //��ȡ�������
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        //��ֵ
        confiner.m_BoundingShape2D = confinerShape;

        confiner.InvalidatePathCache();
    }
}
