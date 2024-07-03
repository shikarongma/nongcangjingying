using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFram.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;//图片的位置
        private BoxCollider2D coll;

        public float gravity = -3.5f;
        private bool isGround;
        private float distance;//距离
        private Vector2 direction;//飞出方向
        private Vector3 targetPos;//飞出位置

        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false; //首先关闭碰撞体，否则刚生成还未落地就可能会被玩家重新拾取
        }

        private void Update()
        {
            Bounce();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">目标位置，即鼠标点击位置</param>
        /// <param name="dir">物品飞动方向</param>
        public void InitBounceItem(Vector3 target,Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);

            spriteTrans.position += Vector3.up * 1.5f;
        }

        //注：阴影的坐标实际上就是整体的坐标 图片的坐标从起始就比整体高1.5；
        private void Bounce()
        {
            isGround = spriteTrans.position.y <= transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                //整体的横坐标移动
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
            }

            if (!isGround)
            {
                //物品图片高度的下降
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;//物品坐标持续下降
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}

