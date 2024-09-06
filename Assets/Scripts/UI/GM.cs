using Constant;
using QEntity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GM : UIEntity
{
    public GameObject GMPanel;
    public Button GMBtn;
    public Text FPS;
    public Text PING;
    InputField input;

    public override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        input = GetComponentInChildren<InputField>();

        float startTime = 0f;
        bool isCounting = false;
        int times = 0;
        GMBtn.onClick.AddListener(() =>
        {
            if(isCounting == false)
            {
                isCounting = true;
                times ++;
                startTime = Time.time;
            }
            else
            {
                float passTime = Time.time - startTime;
                if (passTime > GMConstant.GMTime)
                {
                    times = 1;
                    startTime = Time.time;
                }
                else
                {
                    times ++;
                    if(times >= GMConstant.GMClickTimes)
                    {
                        GMPanel.SetActive(!GMPanel.activeSelf);
                        isCounting = false;
                        times = 0;
                    }
                }
            }
        });
    }

    bool HasShadow = true;
    public void ChangeShadowState()
    {
        NoFocusOnInput();

        Light[] lights = FindObjectsOfType<Light>();
        if (HasShadow)
        {
            foreach (Light light in lights)
            {
                light.shadows = LightShadows.None;
            }
        }
        else
        {
            foreach (Light light in lights)
            {
                light.shadows = LightShadows.Soft;
            }
        }
        HasShadow = !HasShadow;
    }

    // 1,2,3 三档
    public void ChangeQuality()
    {
        NoFocusOnInput();
        if (InputEmpty()) return;

        Ctrl.SetQuality(int.Parse(input.text));
    }

    public void ChangeFrameRate()
    {
        NoFocusOnInput();
        if (InputEmpty()) return;

        Ctrl.SetFrameRate(int.Parse(input.text));
    }

    public void ChangeGround()
    {
        Main.AlwaysGrounds = !Main.AlwaysGrounds;
    }

    public void ChangeMapLength()
    {
        NoFocusOnInput();
        if (InputEmpty()) return;

        Main.MapLength = int.Parse(input.text);
    }

    public void CreateItem()
    {
        NoFocusOnInput();
        if (InputEmpty()) return;

        Ctrl.MakeItemToBag(int.Parse(input.text));
    }

    public void HideOrShowHUD()
    {
        if(HUDManager.SelfShow)
        {
            HUDManager.HideHUD();
        }
        else
        {
            HUDManager.ShowHUD();
        }
    }

    //public void AddItem()
    //{
    //    NoFocusOnInput();
    //    if (InputEmpty()) return;

    //    Bag.Instance.AddItem(int.Parse(input.text));
    //}

    //public void CreateDungeons()
    //{
    //    NoFocusOnInput();
    //    if (InputEmpty()) return;

    //    int num = int.Parse(input.text);
    //    RandomDungeonManager.CreateDungeons(num, LoadTool.LoadGameObjectPrefab(Path.FloorPrefabPath + "DefaultFloor"), LoadTool.LoadGameObjectPrefab(Path.WallPrefabPath + "DefaultWall"), 20, 20, 400);
    //}

    //public void CreateOneLineDungeon()
    //{
    //    Vector3 startPos = RandomDungeonManager.CreateOneLineDungeon(LoadTool.LoadGameObjectPrefab(Path.FloorPrefabPath + "DefaultFloor"), LoadTool.LoadGameObjectPrefab(Path.WallPrefabPath + "DefaultWall"), 100, 12);
    //    Main.MainPlayerCtrl.Position = startPos.CopyAndChangeY(0);
    //}

    //public void DestroyDungeons()
    //{
    //    RandomDungeonManager.ClearAllDungeonsAndHallway();
    //}

    //public void SliderOutTask()
    //{
    //    GUI.SliderOutTask("测试任务", "详细信息", 
    //        () => 
    //        {
    //            GUI.ActionInfoLog("点击了确定");
    //            GUI.SliderInTask();
    //        },
    //        () =>
    //        {
    //            GUI.ActionInfoLog("点击了取消");
    //            GUI.SliderInTask();
    //        }
    //    );
    //}

    //public void SliderInTask()
    //{
    //    GUI.SliderInTask();
    //}

    public bool InputEmpty()
    {
        if (input == null) return true;
        if (input.text == string.Empty) return true;
        return false;
    }

    public void NoFocusOnInput()
    {
        if(input == null)
        {
            input = GetComponentInChildren<InputField>();
        }
        input.DeactivateInputField();
    }
}
