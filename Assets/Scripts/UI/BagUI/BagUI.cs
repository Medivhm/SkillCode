using QEntity;
using Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class BagUI : UIEntity
{
    public Dictionary<int, ItemUI> itemUIs;
    public Dictionary<Item, ItemUI> itemWithGrids;
    public Dictionary<int, TabUI> tabUIs;
    public Transform TabTrans;
    public Transform GridTrans;

    protected override void Awake()
    {
        base.Awake();
        itemUIs = new Dictionary<int, ItemUI>();
        itemWithGrids = new Dictionary<Item, ItemUI>();
        tabUIs = new Dictionary<int, TabUI>();
    }

    public void Start()
    {
        ResetTransform();
        InitUI();
        InitEvent();
        if (Main.LocateUI)
        {
            this.transform.position = (Main.LocateUI.CenterNode.position + Main.LocateUI.RightUpNode.position) / 2;
        }
    }

    private void InitEvent()
    {
        Bag.Instance.ItemChange += (item, ch) =>
        {
            if (this.gameObject.activeSelf)
            {
                RecieveMessage(item, ch);
            }
        };
    }

    private void RecieveMessage(Item item, char ch)
    {
        if (ch == 'a')
        {
            AddItemUI(item);
        }
        else if (ch == 'd')
        {
            ClearItemUI(itemWithGrids[item]);
        }
        else if (ch == '+')
        {
            itemWithGrids[item].RefreshValue();
        }
        else if (ch == '-')
        {
            itemWithGrids[item].RefreshValue();
        }
    }

    public override void RefreshUI()
    {
        if (itemUIs.IsNull() || itemUIs.Count == 0) return;

        base.RefreshUI();
        int itemSetNum = SetItems();
        for (int i = itemSetNum; i < itemUIs.Count; i++)
        {
            itemUIs[i].JustGrid();
        }
    }

    private void InitUI()
    {
        StartCoroutine(InitTabs());
        InitGrids();
        SetItems();
    }

    IEnumerator InitTabs()
    {
        ClearTabs();

        for (int i = 0; i < DataManager.Instance.BagInfos.Tabs.Count; i++)
        {
            string tabTitle = DataManager.Instance.BagInfos.Tabs[i];
            TabUI tab = GUI.CreateTab(tabTitle, null, TabTrans);
            tabUIs.Add(i, tab);
        }

        yield return null;
        TabTrans.ForceRebuildLayout();
    }

    private void ClearTabs()
    {
        foreach (TabUI tab in tabUIs.Values)
        {
            tab.Destroy();
        }
        tabUIs.Clear();
    }

    private void InitGrids()
    {
        ClearGrids();

        AddGrids(DataManager.Instance.BagInfos.InitGrid);
    }

    private void ClearGrids()
    {
        foreach (ItemUI item in itemUIs.Values)
        {
            item.Destroy();
        }
        itemUIs.Clear();
    }

    private void AddGrids(int num)
    {
        for (int i = 0; i < num; i++)
        {
            ItemUI item = GUI.CreateGrid(GridTrans);
            itemUIs.Add(i, item);
        }
    }

    // 返回填入了多少个item
    private int SetItems()
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
                    AddGrids(10);
                }
            }
        }
        return setGridIndex;
    }

    private void AddItemUI(Item item)
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
        AddGrids(10);
        AddItemUI(item);
    }

    private void ClearItemUI(ItemUI ui)
    {
        itemWithGrids.Remove(ui.item);
        ui.JustGrid();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
        this.transform.localPosition = Vector3.zero;
    }
}
