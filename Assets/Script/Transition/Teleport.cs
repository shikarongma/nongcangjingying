using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������
namespace MFarm.Transition
{
    public class Teleport : MonoBehaviour
    {
        public string nextSceneName;//��һ������������
        public Vector3 nextPosition;//������ֵ�λ��

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                //�����л��������¼�
                EventHandler.CallTransition(nextSceneName, nextPosition);
            }
        }
    }
}

