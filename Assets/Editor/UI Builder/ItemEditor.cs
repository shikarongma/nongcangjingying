using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;


public class ItemEditor : EditorWindow
{

    /// <summary>
    /// ���
    /// </summary>
    //�б�������
    private ItemDataList_SO dataBase;
    //���б����ݴ洢,itemList�͵���dataBase�����Ӧ��list
    private List<ItemDetails> itemList = new List<ItemDetails>();
    //�õ�ģ��
    private VisualTreeAsset itemRowTempLate;
    //���VisualElemwnt
    private ListView itemListView;


    /// <summary>
    /// �ұ�
    /// </summary>
    //����ұ�ScrollView
    private ScrollView itemDetailsSection;
    //��õ�����Item��Ϣ
    private ItemDetails activeItem;
    //��ȡĬ�ϵ�ͷ��
    private Sprite defaultIcon;


    [MenuItem("ZH/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("ItemEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //// VisualElements objects can contain other VisualElement following a tree hierarchy.
        //VisualElement label = new Label("Hello World! From C#");
        //root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        /// <summary>
        /// ���
        /// </summary>
        //�õ�ģ������
        itemRowTempLate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRow Template.uxml");
        //���ListView
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        //��������
        LoadDataBase();
        //����List View
        GenerateListView();
        //��������Ʒ��ť
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;

        /// <summary>
        /// �ұ�
        /// </summary>
        //���ScrollView
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //���ɾ����Ʒ��ť
        root.Q<Button>("DeleteButton").clicked += OnDeleteItemClicked;
}


    //�����Ӱ�ť�������¼�
    private void OnAddItemClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "NewItem";
        newItem.itemID = 1000 + itemList.Count;
        itemList.Add(newItem);
        itemListView.Rebuild();
    }
    //���ɾ����ť�������¼�
    private void OnDeleteItemClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    //����ItemDataList_SO
    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");

        if (dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = (ItemDataList_SO) AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO));
        }

        itemList = dataBase.itemDetailsList;
        //�������Ǿ��޷���������
        EditorUtility.SetDirty(dataBase);
        //Debug.Log(itemList[0].itemID);
    }

    //����listView
    private void GenerateListView()
    {
        Func<VisualElement> makeItem = () => itemRowTempLate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if (i < itemList.Count)
            {
                if (itemList[i].itemIcon!=null)
                    e.Q<VisualElement>("Icon").style.backgroundImage = itemList[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemList[i] == null ? "NO ITEM" : itemList[i].itemName;
            }
        };


        itemListView.fixedItemHeight = 60;
        //�����ݸ�ֵ��ListView
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
        //itemListView.selectionType = SelectionType.Multiple;

        //����ǰ�����item��ֵ��activeItem
        itemListView.onSelectionChange += OnListSelectionChange;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails)selectedItem.First();
        GetItemDetails();
        itemDetailsSection.visible = true;
    }

    //����ұ���ϸ����
    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        //��ȡ��Ʒ��Ϣ
        //1.face
        itemDetailsSection.Q<VisualElement>("Icon").style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        //2.ID
        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        //�ص�
        itemDetailsSection.Q<IntegerField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });
        //3.Name
        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });
        //4.Type
        itemDetailsSection.Q<EnumField>("ItemType").Init(activeItem.itemType);
        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });
        //5.Icon
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newSprite = (Sprite)evt.newValue;
            activeItem.itemIcon = newSprite;
            itemDetailsSection.Q<VisualElement>("Icon").style.backgroundImage = newSprite == null ? defaultIcon.texture : newSprite.texture;
            itemListView.Rebuild();
        });
        //6.OnWorld Sprite
        itemDetailsSection.Q<ObjectField>("ItemSprite").value = activeItem.itemOnWorldSprite;
        itemDetailsSection.Q<ObjectField>("ItemSprite").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemOnWorldSprite = (Sprite)evt.newValue;
        });
        //7.Description
        itemDetailsSection.Q<TextField>("Description").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });
        //8.UseRadius
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("ItemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });
        //9.Can PickedUp
        itemDetailsSection.Q<Toggle>("CanPickedup").value = activeItem.canPickedup;
        itemDetailsSection.Q<Toggle>("CanPickedup").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickedup = evt.newValue;
        });
        //10.Can Dropped
        itemDetailsSection.Q<Toggle>("CanDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });
        //11.Can Carried
        itemDetailsSection.Q<Toggle>("CanCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });
        //12.Price
        itemDetailsSection.Q<IntegerField>("Price").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("Price").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });
        //13.SellPrcentage
        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.sellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.sellPercentage = evt.newValue;
        });
    }
}