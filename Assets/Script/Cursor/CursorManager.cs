using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.EventSystems;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    //����״̬�µ����ͼƬ ������ʹ�ù��ߣ���������
    public Sprite normal, tool, seed, item;

    private Sprite currentImage;//��ǰ���ͼƬ

    private Image cursorImage;//image ���

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
        if (!isSleceted)//���û��ѡ���޸ĵ�ǰͼƬΪnormal
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
