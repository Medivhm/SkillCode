using QEntity;
public class SettingUI : UIEntity
{
    public override void ResetTransform()
    {
        base.ResetTransform();
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
