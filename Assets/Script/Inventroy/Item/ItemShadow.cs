using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFram.Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemShadow : MonoBehaviour
    {
        //��ƷͼƬ
        public SpriteRenderer itemSprite;
        //��Ʒ��Ӱ
        private SpriteRenderer shadowSprite;

        private void Awake()
        {
            shadowSprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            shadowSprite.sprite = itemSprite.sprite;
            shadowSprite.color = new Color(0, 0,0, 0.3f);
        }
    }
}

