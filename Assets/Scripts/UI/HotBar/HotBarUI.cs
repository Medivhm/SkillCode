using Constant;
using Manager;
using QEntity;
using System.Collections;
using Tools;
using UnityEngine;

public class HotBarUI : UIEntity
{
    public Sprite normalSprite;
    public Sprite selectedSprite;
    public Transform Grids;

    private int gridsNum => HotBar.Instance.gridsNum;
    private HotBarItem[] items;
    private int lastSelIdx = -1;
    private bool isInit = false;

    protected override void Awake()
    {
        base.Awake();
        Main.HotBarUI = this;
    }
    private void OnDestroy()
    {
        Main.HotBarUI = null;
        MainMouseController.Instance.RemoveMouseScrollChange(MouseScrollChange);
    }

    public void Start()
    {
        InitQuickKey();
        InitEvent();
        ResetTransform();
        //InitItems();
        //MainMouseController.Instance.AddMouseScrollChange(MouseScrollChange);
        StartCoroutine(DeleteWhenMoveToCurrentProject());
    }

    private void InitEvent()
    {
        HotBar.Instance.ItemChange += (item, ch, index) =>
        {
            if (this.gameObject.activeSelf)
            {
                RecieveMessage(item, ch, index);
            }
        };
    }

    private void RecieveMessage(Item item, char ch, int index)
    {
        if (ch == 'a')
        {
            items[index].SetItem(item);
        }
        else if (ch == 'd')
        {
            items[index].RemoveItem();
        }
        else if (ch == '+')
        {
            items[index].RefreshUI();
        }
        else if (ch == '-')
        {
            items[index].RefreshUI();
        }
    }

    public override void RefreshUI()
    {
        if (!isInit) return;

        base.RefreshUI();
        for(int i = 0; i < HotBar.Instance.Items.Length; i++)
        {
            items[i].SetItem(HotBar.Instance.Items[i]);
        }
    }

    public void InitQuickKey()
    {
        if(gridsNum > 0)
        {
            Ctrl.AddKeyPress(KeyCode.Alpha1, () =>
            {
                SelectIndex(0);
            });
        }
        if (gridsNum > 1)
        {
            Ctrl.AddKeyPress(KeyCode.Alpha2, () =>
            {
                SelectIndex(1);
            });
        }
        if (gridsNum > 2)
        {
            Ctrl.AddKeyPress(KeyCode.Alpha3, () =>
            {
                SelectIndex(2);
            });
        }
        if (gridsNum > 3)
        {
            Ctrl.AddKeyPress(KeyCode.Alpha4, () =>
            {
                SelectIndex(3);
            });
        }
        if (gridsNum > 4)
        {
            Ctrl.AddKeyPress(KeyCode.Alpha5, () =>
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
        SelectIndex(0);
        isInit = true;
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

    public Item GetNowSelectedItem()
    {
        if (items.IsNull()) return null;
        if (items[lastSelIdx].IsNull()) return null;

        return items[lastSelIdx].GetItem();
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
