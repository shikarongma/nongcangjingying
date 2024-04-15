using UnityEngine;

//��Ʒ����
[System.Serializable]
public class ItemDetails
{
    public ItemType itemType;

    public int itemID;

    public string itemName;

    [Header("��Ʒͼ��")]
    public Sprite itemIcon;

    [Header("��Ʒ�ڵ��ϵ�ͼ��")]
    public Sprite itemOnWorldSprite;

    public string itemDescription;

    [Header("��Ʒռ�����")]
    public int itemUseRadius;

    [Header("��Ʒ�Ƿ��ʰȡ")]
    public bool canPickedup;

    [Header("��Ʒ�Ƿ�����ӵ�")]
    public bool canDropped;

    [Header("��Ʒ�Ƿ�ɾ�����")]
    public bool canCarried;

    public int itemPrice;

    [Header("���۰ٷֱ�")]
    [Range(0, 1)]
    public float sellPercentage;
}

//չʾ����Ʒ
[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

//��������
[System.Serializable]
public struct AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

//�����и���item��λ��
[System.Serializable]
public class SerialiazbleVector3
{
    public float x, y, z;

    public SerialiazbleVector3(Vector3 position)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

//������item
[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerialiazbleVector3 position;
}