using QEntity;
using Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : UIEntity
{
    public Dictionary<int, ItemUI> itemUIs;
    public Dictionary<Item, ItemUI> itemWithGrids;
    public Dictionary<int, ItemUI> quickUIs;
    public Dictionary<BodyPos, ItemUI> bodyUIs;

    public Text PlayerName;
    public Transform BagGridParent;
    public Transform BagViewContent;
    public Transform QuickGridParent;
    public ItemUI tempItem;

    private bool isInit = false;

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
        ResetSizeDelta();
        ResetAnchoredPosition();
        //this.transform.localPosition = Vector3.zero;
    }

    protected override void Awake()
    {
        base.Awake();
        itemUIs = new Dictionary<int, ItemUI>();
        itemWithGrids = new Dictionary<Item, ItemUI>();
        quickUIs = new Dictionary<int, ItemUI>();
        bodyUIs = new Dictionary<BodyPos, ItemUI>();
        tempItem.SetShowTips(false);
    }

    public void Start()
    {
        ResetTransform();
        InitUI();
        InitEvent();
        //if (Main.LocateUI)
        //{
        //    this.transform.position = (Main.LocateUI.CenterNode.position + Main.LocateUI.RightUpNode.position) / 2;
        //}
        isInit = true;
    }

    private void InitUI()
    {
        InitBagGrids();
        InitQuickGrids();
        InitBodyGrids();
    }

    private void InitEvent()
    {
        Bag.Instance.ItemChange += (item, ch) =>
        {
            if (this.gameObject.activeSelf)
            {
                RecieveBagMessage(item, ch);
            }
        };

        Body.Instance.ItemChange += (item, ch, pos) =>
        {
            if (this.gameObject.activeSelf)
            {
                RecieveBodyMessage(item, ch, pos);
            }
        };

        HotBar.Instance.ItemChange += (item, ch, index) =>
        {
            if (this.gameObject.activeSelf)
            {
                RecieveQuickMessage(item, ch, index);
            }
        };
    }

    public override void RefreshUI()
    {
        if(!isInit) return;

        base.RefreshUI();
        PlayerName.text = Main.UserName; 
        int itemSetNum = SetBagItems();
        // 重刷UI, 相当于重新排序了
        for (int i = itemSetNum; i < itemUIs.Count; i++)
        {
            itemUIs[i].JustGrid();
        }

        ClearBodyGrids();
        SetBodyItems();

        ClearQuickGrids();
        SetQuickItems();
    }

    #region // Body
    private void RecieveBodyMessage(Item item, char ch, BodyPos pos)
    {
        if (ch == 'a')
        {
            SetBodyItem(item, pos);
        }
        else if (ch == 'd')
        {
            ClearBodyItem(pos);
        }
    }

    private void InitBodyGrids()
    {
        AddBodyGrids();
        ClearBodyGrids();
        SetBodyItems();
    }

    public Transform HeadTranc;
    public Transform LeftHeaderTranc;
    public Transform LeftArmTranc;
    public Transform LeftHandsTranc;
    public Transform LegTranc;
    public Transform BodyTranc;
    public Transform RightHeaderTranc;
    public Transform RightArmTranc;
    public Transform RightHandsTranc;
    public Transform FootTranc;
    private void AddBodyGrids()
    {
        bodyUIs.Add(BodyPos.Head       , GetBagSpecialEventItemUI(HeadTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.LeftHeader , GetBagSpecialEventItemUI(LeftHeaderTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.LeftArm    , GetBagSpecialEventItemUI(LeftArmTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.LeftHands  , GetBagSpecialEventItemUI(LeftHandsTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.Leg        , GetBagSpecialEventItemUI(LegTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.Body       , GetBagSpecialEventItemUI(BodyTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.RightHeader, GetBagSpecialEventItemUI(RightHeaderTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.RightArm   , GetBagSpecialEventItemUI(RightArmTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.RightHands , GetBagSpecialEventItemUI(RightHandsTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
        bodyUIs.Add(BodyPos.Foot       , GetBagSpecialEventItemUI(FootTranc).SetLocalScale(0.75f).SetLocalPosition(Vector3.zero));
    }

    private void ClearBodyGrids()
    {
        foreach (var item in bodyUIs.Values.ToList())
        {
            item.JustGrid();
        }
    }

    private void SetBodyItems()
    {
        for(int i = 0; i < Body.Instance.Items.Length; i++)
        {
            if (Body.Instance.Items[i].IsNotNull())
            {
                bodyUIs[(BodyPos)i].SetItem(Body.Instance.Items[i]);
            }
        }
    }

    private void SetBodyItem(Item item, BodyPos pos)
    {
        ClearBodyItem(pos);

        bodyUIs[pos].SetItem(item);
    }

    private void ClearBodyItem(BodyPos pos)
    {
        bodyUIs[pos].JustGrid();
    }
    #endregion


    //------------------------------------------------------------------
    #region   // Bag
    private void RecieveBagMessage(Item item, char ch)
    {
        if (ch == 'a')
        {
            AddBagItemUI(item);
        }
        else if (ch == 'd')
        {
            ClearBagItemUI(itemWithGrids[item]);
        }
        else if (ch == '+')
        {
            if (itemWithGrids[item].item.IsNull())
            {
                itemWithGrids[item].SetItem(item);
            }
            itemWithGrids[item].RefreshUI();
        }
        else if (ch == '-')
        {
            itemWithGrids[item].RefreshUI();
        }
    }

    private void InitBagGrids()
    {
        ClearBagGrids();
        AddBagGrids(DataManager.Instance.BagInfos.InitGrid);
        SetBagItems();
    }

    private void ClearBagGrids()
    {
        foreach (ItemUI item in itemUIs.Values)
        {
            item.Destroy();
        }
        itemUIs.Clear();
    }

    private void AddBagGrids(int num)
    {
        for (int i = 0; i < num; i++)
        {
            ItemUI item = GetBagSpecialEventItemUI(BagGridParent);
            item.SetLocalScale(0.75f);
            itemUIs.Add(i, item);
        }
    }
    // 返回填入了多少个item
    private int SetBagItems()
    {
        itemWithGrids.Clear();

        int setGridIndex = 0;
        int[] itemKeysArray = Bag.Instance.items.Keys.ToArray<int>();
        int[] itemIDArraySort = QUtil.QuickSort(itemKeysArray);
        List<Item> itemList;
        foreach (var keyIndex in itemIDArraySort)
        {
            itemList = Bag.Instance.items[itemKeysArray[keyIndex]];
            for (int i = 0; i < itemList.Count; i++)
            {
                itemUIs[setGridIndex].SetItem(itemList[i]);
                itemWithGrids.Add(itemList[i], itemUIs[setGridIndex]);
                setGridIndex++;

                // 如果不够了，自动加十个格子
                if (setGridIndex >= itemUIs.Count)
                {
                    AddBagGrids(10);
                }
            }
        }
        return setGridIndex;
    }

    private void AddBagItemUI(Item item)
    {
        foreach (var ui in itemUIs.Values)
        {
            if (ui.IsJustGrid())
            {
                ui.SetItem(item);
                itemWithGrids.Add(item, ui);
                return;
            }
        }

        // 如果格子满了
        AddBagGrids(10);
        AddBagItemUI(item);
    }

    private void ClearBagItemUI(ItemUI ui)
    {
        itemWithGrids.Remove(ui.item);
        ui.JustGrid();
    }

    #endregion


    //------------------------------------------------------------------
    #region // HotBar
    private void RecieveQuickMessage(Item item, char ch, int index)
    {
        if (ch == 'a')
        {
            SetQuickItem(item, index);
        }
        else if (ch == 'd')
        {
            ClearQuickItem(index);
        }
        else if (ch == '+')
        {
            quickUIs[index].RefreshUI();
        }
        else if (ch == '-')
        {
            quickUIs[index].RefreshUI();
        }
    }

    private void InitQuickGrids()
    {
        ClearQuickGrids();
        AddQuickGrids(HotBar.Instance.gridsNum);
        SetQuickItems();
    }

    private void ClearQuickGrids()
    {
        foreach (ItemUI item in quickUIs.Values)
        {
            item.JustGrid();
        }
    }

    private void AddQuickGrids(int num)
    {
        for (int i = 0; i < num; i++)
        {
            ItemUI item = GetBagSpecialEventItemUI(QuickGridParent);
            item.SetLocalScale(0.75f);
            quickUIs.Add(i, item);
        }
    }

    private void SetQuickItems()
    {
        for(int i = 0; i < HotBar.Instance.Items.Length; i++)
        {
            if (HotBar.Instance.Items[i].IsNotNull())
            {
                quickUIs[i].SetItem(HotBar.Instance.Items[i]);
            }
        }
    }

    private void SetQuickItem(Item item, int index)
    {
        ClearQuickItem(index);

        quickUIs[index].SetItem(item);
    }

    private void ClearQuickItem(int index)
    {
        quickUIs[index].JustGrid();
    }

    #endregion


    #region // UI Move
    bool onDrag = false;
    ItemUI dragUI = null;
    private ItemUI GetBagSpecialEventItemUI(Transform parent)
    {
        ItemUI item = GUI.CreateItemUI(parent);
        item.BeginDragEvent = (sender, data) =>
        {
            if (sender.IsJustGrid()) return;

            tempItem.gameObject.SetActive(true);
            tempItem.SetItem(item.item);
            tempItem.JustItem();
            sender.JustGrid();
            tempItem.transform.position = sender.transform.position;

            dragUI = sender;
            onDrag = true;
        };
        item.DragEvent = (sender, data) =>
        {
            if (!onDrag) return;

            tempItem.transform.position = data.position;
        };
        item.EndDragEvent = (sender, data) =>
        {
            if (!onDrag) return;

            // 移动背包里的东西
            if (tempItem.item.From.Equals(From.Bag))
            {
                // 是否移到Quick里面
                for(int i = 0; i < quickUIs.Count; i++)
                {
                    if (data.position.PointInRect(quickUIs[i].GetComponent<RectTransform>()))
                    {
                        ReputItem();
                        Bag.Instance.RemoveItemToHotBar(sender.item, i);
                        break;
                    }
                }

                // 是否移到Body里面
                if (onDrag)
                {
                    foreach (var bodyPos in bodyUIs.Keys.ToList())
                    {
                        if (data.position.PointInRect(bodyUIs[bodyPos].GetComponent<RectTransform>()))
                        {
                            ReputItem();
                            Bag.Instance.RemoveItemToBody(sender.item, bodyPos);
                            break;
                        }
                    }
                }
            }
            // 移动身上的东西
            else if (tempItem.item.From.Equals(From.Body))
            {

                if (data.position.PointInRect(BagViewContent.GetComponent<RectTransform>()))
                {
                    ReputItem();
                    Body.Instance.RemoveItemToBag((BodyPos)sender.item.indexMarker);
                }
            }
            // 移动快捷栏里的东西
            else if (tempItem.item.From.Equals(From.HotBar))
            {
                // 往背包里丢
                if (data.position.PointInRect(BagViewContent.GetComponent<RectTransform>()))
                {
                    ReputItem();
                    HotBar.Instance.RemoveItemToBag(sender.item.indexMarker);
                }
                // 往Quick其他格子里丢
                if (onDrag)
                {
                    for (int i = 0; i < quickUIs.Count; i++)
                    {
                        // 跳过自移动
                        if (i == tempItem.item.indexMarker) continue;

                        if (data.position.PointInRect(quickUIs[i].GetComponent<RectTransform>()))
                        {
                            ReputItem();
                            HotBar.Instance.ChangeWithOtherGrid(sender.item.indexMarker, i);
                            break;
                        }
                    }
                }
            }

            ReputItem();
        };
        return item;
    }

    private void ReputItem()
    {
        if (onDrag)
        {
            dragUI.SetItem(tempItem.item);
            tempItem.JustGrid();
            tempItem.gameObject.SetActive(false);

            dragUI = null;
            onDrag = false;
        }
    }

    #endregion

    public override void Hide()
    {
        base.Hide();
        ReputItem();     // 如果在拖动的时候退出
    }
}
