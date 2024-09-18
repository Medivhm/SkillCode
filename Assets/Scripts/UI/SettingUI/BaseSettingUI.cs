using QEntity;
using UnityEngine.UI;

public class BaseSettingUI : UIEntity
{
    public Toggle playBackgroundMusicToggle;

    protected override void Awake()
    {
        base.Awake();
        playBackgroundMusicToggle.isOn = Main.PlayBackgroundMusic;
        playBackgroundMusicToggle.onValueChanged.AddListener((bool call) =>
        {
            Main.PlayBackgroundMusic = call;
        });
    }

    private void Start()
    {
        ResetTransform();
    }

    public override void ResetTransform()
    {
        base.ResetTransform();
        ResetScale();
        ResetSizeDelta();
        ResetAnchoredPosition();
    }
}
