using UnityEngine;
using UnityEngine.UI;
using UnityEditor.EventSystems;
using UnityEngine.EventSystems;
using MFarm.Map;
using MFarm.CropPlant;

public class CursorManager : MonoBehaviour
{
    //����״̬�µ����ͼƬ ������ʹ�ù��ߣ���������
    public Sprite normal, tool, seed, item;

    private Sprite currentImage;//��ǰ���ͼƬ

    private Image cursorImage;//image ���

    private RectTransform cursorCanvas;

    private Transform playerTransform => GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    //�����
    private Camera mainCamera;
    private Grid currentGrid;

    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;

    private bool cursorEnable;
    //��ǰ���λ���Ƿ����
    private bool cursorPositionValid;

    //��ǰ��Ʒ
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

    //��굱ǰ����
    private void SetCursorValid()
    {
        cursorImage.color = new Color(1, 1, 1, 1);
        cursorPositionValid = true;
    }

    //��겻����
    private void SetCursorInValid()
    {
        cursorImage.color = new Color(1, 0, 0, 0.4f);
        cursorPositionValid = false;
    }

    /// <summary>
    /// ����ѡ�����Ʒ�������ͼ��
    /// </summary>
    /// <param name="itemDetails"></param>
    /// <param name="isSleceted"></param>
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSleceted)
    {

        if (!isSleceted)//���û��ѡ���޸ĵ�ǰͼƬΪnormal
        {
            currenmtItemDetails = null;
            cursorEnable = false;
            currentImage = normal;
        }
        else//��Ʒ��ѡ�в��л�ͼƬ
        {
            currenmtItemDetails = itemDetails;
            //TODO:����������Ͷ�ӦͼƬ
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

    //����������Ʒ����
    private void CheckPlayerInput()
    {
        if (Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currenmtItemDetails);
        }
    }

    //�������״̬
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
                //TODO:�������ͻ�δд
                case ItemType.Seed://����
                    if(currentTile.digDays>-1&&currentTile.seedItemID==-1)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.Commodity://��Ʒ
                    if (currentTile.canDropItem)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.HoeTool://��ͷ��
                    if (currentTile.canDig)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.WaterTool://��ˮ
                    if (currentTile.digDays > -1 && currentTile.waterDays == -1)
                        SetCursorValid();
                    else
                        SetCursorInValid();
                    break;
                case ItemType.BreakTool://����
                case ItemType.ChopTool://��ͷ
                    if (crop != null)
                    {
                        if (crop.CanHarvest && crop.cropDetails.CheckToolAvailable(currenmtItemDetails.itemID))
                            SetCursorValid();
                        else SetCursorInValid();
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.CollectTool://������
                    if (currentCrop != null)
                    {
                        if (currentCrop.CheckToolAvailable(currenmtItemDetails.itemID))
                        {
                            if (currentTile.growthDays >= currentCrop.TotalGrowthDays)//ũ�������֮��ſ����ջ�
                                SetCursorValid();
                            else SetCursorInValid();
                        }
                    }
                    else
                        SetCursorInValid();
                    break;
                case ItemType.ReapTool://���
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


    //�ж�����Ƿ���λ��UI������
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
