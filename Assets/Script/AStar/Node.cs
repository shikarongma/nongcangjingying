using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.AStar
{
    //����ڵ�
    public class Node : IComparable<Node>//�Ƚ�ֵ������Ƚϵ���node
    {
        public Vector2Int gridPosition;//�������꣨������������ԣ�0.0��Ϊԭ�������ϵ�µ����꣬����ʵ�ʳ��������꣩
        public int gCost = 0;//����start���ӵľ���
        public int hCost = 0;//����Target���ӵľ���
        public int FCost => gCost + hCost;//��ǰ���ӵ�ֵ
        public bool isObstacle = false;//��ǰ�����Ƿ����ϰ�
        public Node parentNode;//���ڵ�

        public Node(Vector2Int pos)
        {
            gridPosition = pos;
            parentNode = null;
        }

        public int CompareTo(Node other)
        {
            //�Ƚ�ѡ����͵�Fֵ������-1��0��1
            int result = FCost.CompareTo(other.FCost);
            if (result == 0)
            {
                result = hCost.CompareTo(other.hCost);
            }
            return result;

        }
    }
}

