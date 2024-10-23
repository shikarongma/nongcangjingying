using UnityEngine;
using UnityEngine.UI;
using UnityEditor.EventSystems;
using UnityEngine.EventSystems;
using MFarm.Map;
using MFarm.CropPlant;

public class CursorManager : MonoBehaviour
{
    //各种状态下的鼠标图片 正常，使用工具，播撒种子
    public Sprite normal, tool, seed, item;

    private Sprite currentImage;//当前鼠标图片

    private Image cursorImage;//image 组件

    private RectTransform cursorCanvas;

    private Transform playerTransform => GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    //鼠标检测
    private Camera mainCamera;
    private Grid currentGrid;

    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;

    private bool cursorEnable;
    //当前鼠标位置是否可用
    private bool cursorPositionValid;

    //当前物品
    private ItemDetails currenmtItemDetails;

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
    }



    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();

        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        currentImage = normal;
        SetCursorImage(normal);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cursorCanvas == null)
            return;
        cursorImage.transform.position = Input.mousePosition;

        if (!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentImage);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
            SetCursorImage(normal);
    }

    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    //鼠标当前可用
    private void SetCursorValid()
    {
        cursorImage.color = new Color(1, 1, 1, 1);
        cursorPositionValid = true;
    }

    //鼠标不可用
    private void SetCursorInValid()
    {
        cursorImage.color = new Color(1, 0, 0, 0.4f);
        cursorPositionValid = false;
    }

    /// <summary>
    /// 根据选择的物品更换鼠标图像
    /// </summary>
    /// <param name="itemDetails"></param>
    /// <param name="isSleceted"></param>
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSleceted)
    {

        if (!isSleceted)//如果没有选择，修改当前图片为normal
        {
            currenmtItemDetails = null;
            cursorEnable = false;
            currentImage = normal;
        }
        else//物品被选中才切换图片
        {
            currenmtItemDetails = itemDetails;
            //TODO:添加所有类型对应图片
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
            cursorEnable = true;

        }
    }

    private void OnBeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void OnAfterSceneUnloadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }

    //鼠标点击完成物品动作
    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currenmtItemDetails);
        }
    }

    //设置鼠标状态
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(playerTransform.position);

        if (Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currenmtItemDetails.itemUseRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currenmtItemDetails.itemUseRadius)
        {
            SetCursorInValid();
            return;
        }

        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);


        if (currentTile != null)
        {
            CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
            Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);
            switch (currenmtItemDetails.itemType)
            {
                //TODO:其他类型还未写
                case ItemType.Seed://种子
                    if(currentTile.digDays>-1&&currentTile.seedItemID==-1)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.Commodity://商品
                    if (currentTile.canDropItem)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.HoeTool://锄头挖
                    if (currentTile.canDig)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.WaterTool://浇水
                    if (currentTile.digDays > -1 && currentTile.waterDays == -1)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.BreakTool://敲子
                case ItemType.ChopTool://斧头
                    if (crop != null)
                    {
                        if (crop.CanHarvest && crop.cropDetails.CheckToolAvailable(currenmtItemDetails.itemID))
                            SetCursorValid();
                        else SetCursorInValid();
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.CollectTool://菜篮子
                    if (currentCrop != null)
                    {
                        if (currentCrop.CheckToolAvailable(currenmtItemDetails.itemID))
                        {
                            if (currentTile.growthDays >= currentCrop.TotalGrowthDays)//农作物成熟之后才可以收获
                                SetCursorValid();
                            else SetCursorInValid();
                        }
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.ReapTool://割草
                    if (GridMapManager.Instance.HaveReapableItemsInRadius(mouseWorldPos, currenmtItemDetails))
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                default:
                    SetCursorInValid();
                    break;
            }
        }
        else
            SetCursorInValid();
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
