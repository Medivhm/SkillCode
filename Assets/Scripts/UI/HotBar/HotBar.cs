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

    protected override void Awake()
    {
        base.Awake();
        Main.HotBar = this;
    }

    public void Start()
    {
        InitQuickKey();
        ResetTransform();
        //InitItems();
        //MainMouseController.Instance.AddMouseScrollChange(MouseScrollChange);
        StartCoroutine(DeleteWhenMoveToCurrentProject());
    }

    public void InitQuickKey()
    {
        if(gridsNum > 0)
        {
            Ctrl.SetQuickKey(KeyCode.Alpha1, () =>
            {
                SelectIndex(0);
            });
        }
        if (gridsNum > 1)
        {
            Ctrl.SetQuickKey(KeyCode.Alpha2, () =>
            {
                SelectIndex(1);
            });
        }
        if (gridsNum > 2)
        {
            Ctrl.SetQuickKey(KeyCode.Alpha3, () =>
            {
                SelectIndex(2);
            });
        }
        if (gridsNum > 3)
        {
            Ctrl.SetQuickKey(KeyCode.Alpha4, () =>
            {
                SelectIndex(3);
            });
        }
        if (gridsNum > 4)
        {
            Ctrl.SetQuickKey(KeyCode.Alpha5, () =>
            {
                SelectIndex(4);
            });
        }
    }

    IEnumerator DeleteWhenMoveToCurrentProject()
    {
        yield return new WaitForSeconds(0.3f);
        InitItems();
        MainMouseController.Instance.AddMouseScrollChange(MouseScrollChange);
        yield return new WaitForSeconds(0.3f);
        items[0].SetUseable(Ctrl.CreateWeapon(1));
        items[1].SetUseable(Ctrl.CreateWeapon(2));
        items[2].SetUseable(Ctrl.CreateWeapon(3));
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
            items[i].selectedIcon.sprite = selectedSprite;
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

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }
}
