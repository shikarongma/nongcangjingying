using MFram.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemID;

    private SpriteRenderer spriteRenderor;
    private ItemDetails itemDetails;

    //实时更改碰撞体
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        spriteRenderor = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        if (itemID != 0)
        {
            Init(itemID);
        }
    }

    public void Init(int ID)
    {
        itemID = ID;
        //inventory 获得当前数据
        itemDetails = InventroyManager.Instance.GetItemDetails(itemID);

        if (itemDetails != null)
        {
            spriteRenderor.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;

            Vector2 newSize = new Vector2(spriteRenderor.sprite.bounds.size.x, spriteRenderor.sprite.bounds.size.y);
            boxCollider.size = newSize;
            boxCollider.offset = new Vector2(0, spriteRenderor.sprite.bounds.center.y);
        }
    }
}
