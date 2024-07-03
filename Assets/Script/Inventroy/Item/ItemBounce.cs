using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFram.Inventory
{
    public class ItemBounce : MonoBehaviour
    {
        private Transform spriteTrans;//ͼƬ��λ��
        private BoxCollider2D coll;

        public float gravity = -3.5f;
        private bool isGround;
        private float distance;//����
        private Vector2 direction;//�ɳ�����
        private Vector3 targetPos;//�ɳ�λ��

        private void Awake()
        {
            spriteTrans = transform.GetChild(0);
            coll = GetComponent<BoxCollider2D>();
            coll.enabled = false; //���ȹر���ײ�壬��������ɻ�δ��ؾͿ��ܻᱻ�������ʰȡ
        }

        private void Update()
        {
            Bounce();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">Ŀ��λ�ã��������λ��</param>
        /// <param name="dir">��Ʒ�ɶ�����</param>
        public void InitBounceItem(Vector3 target,Vector2 dir)
        {
            coll.enabled = false;
            direction = dir;
            targetPos = target;
            distance = Vector3.Distance(target, transform.position);

            spriteTrans.position += Vector3.up * 1.5f;
        }

        //ע����Ӱ������ʵ���Ͼ������������ ͼƬ���������ʼ�ͱ������1.5��
        private void Bounce()
        {
            isGround = spriteTrans.position.y <= transform.position.y;
            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                //����ĺ������ƶ�
                transform.position += (Vector3)direction * distance * -gravity * Time.deltaTime;
            }

            if (!isGround)
            {
                //��ƷͼƬ�߶ȵ��½�
                spriteTrans.position += Vector3.up * gravity * Time.deltaTime;//��Ʒ��������½�
            }
            else
            {
                spriteTrans.position = transform.position;
                coll.enabled = true;
            }
        }
    }
}

