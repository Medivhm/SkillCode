using QEntity;
using System.Collections.Generic;
using Tools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SettingUI : UIEntity
{
    Transform lastShow;

    public Transform MenuItemsParent;
    public Transform PageParent;

    [SerializeField] List<string> PageNames;
    [SerializeField] List<GameObject> Pages;
    Dictionary<string, Transform> pairs;

    protected override void Awake()
    {
        base.Awake();
        MenuItemsParent.RemoveAllChildren();
        PageParent.RemoveAllChildren();
        pairs = new Dictionary<string, Transform>();
    }

    private void Start()
    {
        ResetTransform();
        if (PageNames.Count != Pages.Count) return;

        for(int i = 0; i < Pages.Count; i++)
        {
            GameObject page = Pages[i];
            QButton btn = GetButton();
            btn.SetParent(MenuItemsParent);
            btn.SetText(PageNames[i]);
            btn.SetHeight(100f);
            btn.SetFontSize(45);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if(!pairs.ContainsKey(page.name))
                {
                    Transform trans = GameObject.Instantiate(page).transform;
                    trans.SetParent(PageParent);
                    pairs.Add(page.name, trans);
                }
                ShowPage(pairs[page.name]);
            });
        }

        // 自动按下第一个
        if(Pages.Count > 0)
        {
            Transform trans = GameObject.Instantiate(Pages[0]).transform;
            trans.SetParent(PageParent);
            pairs.Add(Pages[0].name, trans);
            ShowPage(pairs[Pages[0].name]);
        }
    }

    void ShowPage(Transform page)
    {
        if (page == lastShow) return;

        if (lastShow.IsNotNull())
        {
            lastShow.gameObject.SetActive(false);
        }
        page.SetAsLastSibling();
        page.gameObject.SetActive(true);
        lastShow = page;
    }

    QButton GetButton()
    {
        return LoadTool.LoadWidget("QButton").GetComponent<QButton>();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
        ResetAnchoredPosition();
        ResetSizeDelta();
    }

    public override void Show()
    {
        base.Show();
        Manager.UIManager.UIPush(this);
    }

    public override void Hide()
    {
        base.Hide();
        Manager.UIManager.UIPop();
    }
} 
