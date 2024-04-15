using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//传送门
namespace MFarm.Transition
{
    public class Teleport : MonoBehaviour
    {
        public string nextSceneName;//下一个场景的名称
        public Vector3 nextPosition;//人物出现的位置

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                //呼叫切换场景的事件
                EventHandler.CallTransition(nextSceneName, nextPosition);
            }
        }
    }
}

