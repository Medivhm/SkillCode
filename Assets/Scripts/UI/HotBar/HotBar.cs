using Constant;
using Manager;
using QEntity;
using System.Collections;
using Tools;
using UnityEngine;

public class HotBar : UIEntity
{
    public int gridsNum = 5;
    public Sprite normalSprite;
    public Sprite selectedSprite;
    public Transform Grids;

    private HotBarItem[] items;
    private int lastSelIdx = -1;

    public void Awake()
    {
        Main.HotBar = this;
    }

    public void Start()
    {
        //InitItems();
        //MainMouseController.Instance.AddMouseScrollChange(MouseScrollChange);
        StartCoroutine(DeleteWhenMoveToCurrentProject());
    }

    IEnumerator DeleteWhenMoveToCurrentProject()
    {
        yield return new WaitForSeconds(0.3f);
        InitItems();
        MainMouseController.Instance.AddMouseScrollChange(MouseScrollChange);
        yield return new WaitForSeconds(0.3f);
        items[0].SetItem(Ctrl.CreateItem(7));
        items[1].SetItem(Ctrl.CreateItem(8));
        items[2].SetItem(Ctrl.CreateItem(9));
        SelectIndex(0);
    }

    private void OnDestroy()
    {
        Main.HotBar = null;
        MainMouseController.Instance.RemoveMouseScrollChange(MouseScrollChange);
    }

    void InitItems()
    {
        items = new HotBarItem[gridsNum];
        for(int i = 0; i < gridsNum; i++)
        {
            items[i] = GetItem();
            items[i].Selected = false;
            items[i].bgIcon.sprite = normalSprite;
            items[i].transform.SetParent(Grids);
        }
    }

    void SelectIndex(int selIdx)
    {
        if(selIdx < 0 || selIdx > gridsNum - 1) return;
        if(lastSelIdx == selIdx) return;

        if(lastSelIdx >= 0)
        {
            items[lastSelIdx].Selected = false;
        }
        items[selIdx].Selected = true;
        lastSelIdx = selIdx;
    }

    public void SelectMoveRight()
    {
        SelectIndex((lastSelIdx + 1) % gridsNum);
    }

    public void SelectMoveLeft()
    {
        SelectIndex((lastSelIdx + gridsNum - 1) % gridsNum);
    }

    void MouseScrollChange(float delta)
    {
        if (UIManager.HasUI()) return;

        if (delta < 0)
        {
            SelectMoveRight();
        }
        else
        {
            SelectMoveLeft();
        }
    }

    HotBarItem GetItem()
    {
        return LoadTool.LoadUI(UIConstant.HotBarItem).GetComponent<HotBarItem>();
    }
}
