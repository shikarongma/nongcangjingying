using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.EventSystems;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    //各种状态下的鼠标图片 正常，使用工具，播撒种子
    public Sprite normal, tool, seed, item;

    private Sprite currentImage;//当前鼠标图片

    private Image cursorImage;//image 组件

    private RectTransform cursorCanvas;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSleceted)
    {
        if (!isSleceted)//如果没有选择，修改当前图片为normal
        {
            currentImage = normal;
        }
        else
        {
            currentImage = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.CollectTool => tool,
                ItemType.HoeTool => tool,
                ItemType.ReapTool => tool,
                ItemType.WaterTool => tool,
                ItemType.ChopTool => tool,
                ItemType.BreakTool => tool,
                ItemType.Furniture => tool,
                _ => normal
            } ;
        }
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();

        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentImage = normal;
        SetCursorImage(normal);

    }

    private void Update()
    {
        if (cursorCanvas == null)
            return;
        cursorImage.transform.position = Input.mousePosition;

        if (!InteractWithUI())
        {
            SetCursorImage(currentImage);
        }
        else
            SetCursorImage(normal);
    }

    private void SetCursorImage(Sprite sprite)
    {
        currentImage = sprite;
        cursorImage.sprite = currentImage;
    }

    //判断鼠标是否移位到UI界面上
    private bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
            return false;
    }
}
