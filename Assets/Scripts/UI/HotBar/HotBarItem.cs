using QEntity;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class HotBarItem : UIEntity
{
    public Image bgIcon;
    public Image itemIcon;
    private Useable thing;
    private bool selected;

    public bool Selected
    {
        get { return selected; }
        set
        {
            if (value == selected) return;

            selected = value;
            if (selected)
            {
                bgIcon.sprite = Main.HotBar.selectedSprite;
                if (thing.IsNotNull())
                {
                    thing.Use();
                }
            }
            else
            {
                bgIcon.sprite = Main.HotBar.normalSprite;
            }
        }
    }

    private void Start()
    {
        ResetTransform();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
    }

    public void RemoveUseable()
    {
        thing = null;
        itemIcon.sprite = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void SetUseable(Useable thing)
    {
        if (this.thing.IsNotNull()) return;

        this.thing = thing;
        itemIcon.sprite = LoadTool.LoadSprite(thing.Icon);
        itemIcon.gameObject.SetActive(true);
    }

    public Useable GetUseable()
    {
        return thing;
    }
}
