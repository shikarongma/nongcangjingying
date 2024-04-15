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
    /// 左边
    /// </summary>
    //列表获得数据
    private ItemDataList_SO dataBase;
    //将列表数据存储,itemList就等于dataBase里面对应的list
    private List<ItemDetails> itemList = new List<ItemDetails>();
    //拿到模板
    private VisualTreeAsset itemRowTempLate;
    //获得VisualElemwnt
    private ListView itemListView;


    /// <summary>
    /// 右边
    /// </summary>
    //获得右边ScrollView
    private ScrollView itemDetailsSection;
    //获得单独的Item信息
    private ItemDetails activeItem;
    //获取默认的头像
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
        /// 左边
        /// </summary>
        //拿到模板数据
        itemRowTempLate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRow Template.uxml");
        //获得ListView
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        //加载数据
        LoadDataBase();
        //生成List View
        GenerateListView();
        //获得添加物品按钮
        root.Q<Button>("AddButton").clicked += OnAddItemClicked;

        /// <summary>
        /// 右边
        /// </summary>
        //获得ScrollView
        itemDetailsSection = root.Q<ScrollView>("ItemDetails");
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //获得删除物品按钮
        root.Q<Button>("DeleteButton").clicked += OnDeleteItemClicked;
}


    //点击添加按钮发生的事件
    private void OnAddItemClicked()
    {
        ItemDetails newItem = new ItemDetails();
        newItem.itemName = "NewItem";
        newItem.itemID = 1000 + itemList.Count;
        itemList.Add(newItem);
        itemListView.Rebuild();
    }
    //点击删除按钮发生的事件
    private void OnDeleteItemClicked()
    {
        itemList.Remove(activeItem);
        itemListView.Rebuild();
        itemDetailsSection.visible = false;
    }

    //查找ItemDataList_SO
    private void LoadDataBase()
    {
        var dataArray = AssetDatabase.FindAssets("ItemDataList_SO");

        if (dataArray.Length > 1)
        {
            var path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
            dataBase = (ItemDataList_SO) AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_SO));
        }

        itemList = dataBase.itemDetailsList;
        //如果不标记就无法保存数据
        EditorUtility.SetDirty(dataBase);
        //Debug.Log(itemList[0].itemID);
    }

    //生成listView
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
        //将数据赋值给ListView
        itemListView.itemsSource = itemList;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;
        //itemListView.selectionType = SelectionType.Multiple;

        //将当前点击的item赋值给activeItem
        itemListView.onSelectionChange += OnListSelectionChange;
    }

    private void OnListSelectionChange(IEnumerable<object> selectedItem)
    {
        activeItem = (ItemDetails)selectedItem.First();
        GetItemDetails();
        itemDetailsSection.visible = true;
    }

    //获得右边详细数据
    private void GetItemDetails()
    {
        itemDetailsSection.MarkDirtyRepaint();

        //获取物品信息
        //1.face
        itemDetailsSection.Q<VisualElement>("Icon").style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
        //2.ID
        itemDetailsSection.Q<IntegerField>("ItemID").value = activeItem.itemID;
        //回调
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