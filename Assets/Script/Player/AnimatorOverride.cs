using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFram.Inventory;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animator;
    //��þ����ͼƬ
    public SpriteRenderer holdItem;

    [Header("�����ֶ����б�")]
    public List<AnimatorType> animatorTypes;

    //�ֵ�,�洢��Ҹ������ֶ��Ӧ��animator
    private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();

    private void Awake()
    {
        animator = GetComponentsInChildren<Animator>();

        foreach(var anim in animator)
        {
            animatorNameDict.Add(anim.name, anim);
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnHarvestAtPlayerPosition(int itemID)
    {
        Sprite itemSprite = InventroyManager.Instance.GetItemDetails(itemID).itemOnWorldSprite;
        if (!holdItem.enabled)
            StartCoroutine(ShowItem(itemSprite));
    }

    private IEnumerator ShowItem(Sprite itemSprite)
    {
        holdItem.enabled = true;
        holdItem.sprite = itemSprite;
        yield return new WaitForSeconds(1f);
        holdItem.enabled = false;
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        //TODO������������
        PartType currentType = itemDetails.itemType switch
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.HoeTool => PartType.Hoe,
            ItemType.WaterTool => PartType.Water,
            ItemType.CollectTool => PartType.Collect,
            ItemType.ChopTool => PartType.Chop,
            ItemType.BreakTool => PartType.Break,
            ItemType.ReapTool => PartType.Reap,
            _ => PartType.None
        };

        //�ٲ�����Ʒ���ж�
        if (isSelected == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if (currentType == PartType.Carry)
            {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
            else
                holdItem.enabled = false;
        }

        SwitchAnimator(currentType);

    }

    private void OnBeforeSceneUnloadEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach(var item in animatorTypes)
        {
            if (item.partType == partType)
            {
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
