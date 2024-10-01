using QEntity;
using Manager;
using UnityEngine;
using System;
using System.Collections.Generic;
using Fsm;
using System.Collections;
using Constant;
using Tools;

public enum MoveType
{
    SkillMove,
    UserMove,
    AutoMove,
}

public class PlayerController : PlayerEntity
{
    public CheckGround checkGround;
    public Transform magicShootPoint;

    public float speed = 10f;
    public float jumpSpeed = 26f;
    public float moveDistanceTick = 10f;      // 移动多长距离做一次距离相关Tick
    public SkillFsm skillFsm;

    public Action<float, Vector3> MoveTickAction;    // arg1:move distance, arg2:position

    private Profession profession;
    public override Profession Profession 
    {
        get
        {
            return profession;
        }
        set
        {
            if(Profession.Wizard == profession)
            {
                spelling = false;
                if (Main.MagicProgressbar.gameObject.activeSelf)
                {
                    Main.MagicProgressbar.ResetData();
                    Main.MagicProgressbar.Hide();
                }
            }
            else if (Profession.Shooter == profession)
            {
                shooting = false;
                Main.MainCameraCtrl.SetCameraView(CameraView.ThirdPerson);
            }
            profession = value;
        }
    }

    private bool isMoving;
    public bool IsMoving
    {
        get { return isMoving; }
        set 
        {
            if(isMoving == value)
            {
                return;
            }

            isMoving = value;
            if(isMoving == true)
            {
                PlayAnim("Run", 0.01f);
            }
            else
            {
                PlayAnim("Idle", 0.01f);
            }
        }
    }

    bool isJump = false;
    public override bool IsJump
    {
        get
        {
            return isJump;
        }
        set
        {
            isJump = value;
        }
    }

    public override float JumpSpeed => jumpSpeed;

    public override bool IsGrounded
    {
        get
        {
            if (Main.AlwaysGrounds)
            {
                return true;
            }
            return checkGround.IsGrounded;
        }
    }

    private void OnDestroy()
    {
        Main.MainPlayerCtrl = null;
    }

    void Start()
    {
        Main.MainPlayerCtrl = this;
        camp = new Camp { CampID = 0 };
        gameObject.name = string.Format("[{0}]", 0);
        //hud = Ctrl.AddHUD(this.transform, string.Format("[{0}] {1}", 1, "测试玩家"), 100f, 50f);
        InitPropers();
        InitHUD();
        DetectInit();
        EventInit();
        KeyboardPressInit();
        SkillInit();
        MouseClickInit();

        MoveTickAction += ChunkManager.Instance.CheckPlayerPos;
        Test();

        Profession = Profession.Fighter;
    }

    protected override void Update()
    {
        base.Update();
        if(!banControl)
        {
            MoveTick();
            RotateTick();
        }
        if (skillFsm.IsNotNull())
        {
            skillFsm.Update(Time.deltaTime);
        }
    }

    
    void DetectInit()                    // 初始化距离内人物检测
    #region
    {
        detect = new DetectHelper();
        detect.Init(this, 25, "Unit");
        detect.AddTargetEnterEvent((GameObject go) =>
        {
            GUI.ActionInfoLog(string.Format("[{0}] 进入检测范围", go.name));
        }); 
        detect.AddTargetLeaveEvent((GameObject go) =>
        {
            GUI.ActionInfoLog(string.Format("[{0}] 离开", go.name));
        });
    }
    #endregion

    void KeyboardPressInit()             // 快捷键初始化
    #region
    {
        //    Ctrl.SetQuickKey(KeyCode.Alpha1, () =>
        //    {
        //        PlayAnim("Dance1", 0.1f);
        //    });
        //    Ctrl.SetQuickKey(KeyCode.Alpha2, () =>
        //    {
        //        PlayAnim("Dance2", 0.1f);
        //    });
        //    Ctrl.SetQuickKey(KeyCode.Alpha3, () =>
        //    {
        //        PlayAnim("Dance3", 0.1f);
        //    });
        Ctrl.SetQuickKey(KeyCode.Space, () =>
        {
            if (IsGrounded)
            {
                IsJump = true;
                SetJumpSpeed();           // 设置跳跃速度
            }
        });
        Ctrl.SetQuickKey(KeyCode.R, () =>
        {
            Position = new Vector3(0, 10, 0);
        });
    }
    #endregion

    private void OnDrawGizmos()          // 画近战攻击区域框
    #region
    {
        if (Profession.Fighter == this.Profession)
        {
            Gizmos.color = Color.red;

            // 在世界坐标系中绘制一个代表 OverlapBox 的线框盒子
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + this.transform.forward * 1.5f + this.transform.up * 2, this.transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.5f, 1.5f, 1.5f) * 2); // 使用 WireCube 绘制线框盒子
        }
    }
    #endregion

    void MouseClickInit()                // 鼠标点击初始化
    #region
    {
        // RaycastHit[]
        MainMouseController.Instance.AddMouseLeftDown(() =>
        {
            if (UIManager.HasUI()) return;
            // 法师
            if (Profession.Wizard == this.Profession)
            {
                if (spelling) return;

                spelling = true;
                Main.MagicProgressbar.Show();
                Main.MagicProgressbar.StartProgressBar(0.5f);
            }
            // 战士
            else if(Profession.Fighter == this.Profession)
            {
                List<UnitEntity> units = this.gameObject.FindUnitInBox(this.transform.position + this.transform.forward * 1.5f + this.transform.up * 2, new Vector3(1.5f, 1.5f, 1.5f), this.transform.rotation);
                foreach (var unit in units)
                {
                    if (this.gameObject.IsNotMe(unit.gameObject))
                    {
                        unit.GetHurt(20f);
                    }
                }
                GUI.ActionInfoLog(string.Format("攻击到了 {0} 个人", (units.Count - 1).ToString()));
            }
            // 射手
            else if(Profession.Shooter == this.Profession)
            {
                if (shooting) return;

                shooting = true;
                Main.MainCameraCtrl.SetCameraView(CameraView.OverShoulder);
            }
        });


        MainMouseController.Instance.AddMouseLeftUp(() =>
        {
            // 法师
            if (Profession.Wizard == this.Profession)
            {
                spelling = false;
                if (Main.MagicProgressbar.ChargeOver)
                {
                    Vector3 shootDir = Main.MainCamera.transform.forward;
                    Vector3? hitPoint = GetClosestRayHitPoint();
                    if (hitPoint.IsNotNull())
                    {
                        shootDir = (Vector3)hitPoint - magicShootPoint.position;
                    }
                    UseMagic(magicShootPoint.position, shootDir, hitPoint);
                }
                Main.MagicProgressbar.Hide();
                Main.MagicProgressbar.ResetData();
            }
            // 战士
            else if (Profession.Fighter == this.Profession)
            {
            }
            // 射手
            else if (Profession.Shooter == this.Profession)
            {
                if (!UIManager.HasUI())
                {
                    ShootArrow();
                }
                Main.MainCameraCtrl.SetCameraView(CameraView.ThirdPerson);
                shooting = false;
            }
        });
    }
    #endregion

    
    public Vector3? GetClosestRayHitPoint()        // 获取击中最近的点
    #region
    {
        var infos = MainMouseController.Instance.GetCenterScreenRayHit();
        Vector3? hitPoint = null;
        float minDis = float.MaxValue;
        for (int i = infos.Length - 1; i >= 0; i--)
        {
            if (this.gameObject.IsMe(infos[i].collider.gameObject))
                continue;

            if (infos[i].collider.gameObject.CompareTag(TagConstant.Detect))
                continue;


            float nowDis = Vector3.Distance(infos[i].point, magicShootPoint.position);
            if (nowDis < minDis)
            {
                minDis = nowDis;
                hitPoint = infos[i].point;
            }
        }
        return hitPoint;
    }
    #endregion

    Magic.Magic magic1;
    Magic.Magic magic2;
    Magic.Magic magic3;
    Magic.Magic magic4;
    void Test()
    #region
    {
        magic1 = Magic.Magic.GetAMagic(this);
        magic1.SetCarrier(3, 1);  // 圆柱
        magic1.SetMagicSkill(1);  // 变大
        magic1.SetMagicSkill(2);  // 变强
        magic1.SetMagicSkill(7);  // 定点
        magic1.SetMagicSkill(8);  // 自动寻敌
        magic1.SetMagicSkill(2);  // 变强
        magic1.SetMagicSkill(2);  // 变强
        magic1.SetMagicSkill(2);  // 变强


        magic2 = Magic.Magic.GetAMagic(this);
        magic2.SetCarrier(2, 1);  // sphere
        magic2.SetMagicSkill(3);  // 变快
        magic2.SetMagicSkill(3);  // 变快
        magic2.SetMagicSkill(3);  // 变快
        magic2.SetMagicSkill(3);  // 变快
        magic2.SetMagicSkill(2);  // 变强
        magic2.SetMagicSkill(2);  // 变强
        magic2.SetMagicSkill(2);  // 变强
        magic2.SetMagicSkill(2);  // 变强


        magic3 = Magic.Magic.GetAMagic(this);
        magic3.SetCarrier(1, 1);  // cube
        magic3.SetMagicSkill(4);  // 2分裂
        magic3.SetMagicSkill(5);  // 3分裂
        

        magic4 = Magic.Magic.GetAMagic(this);
        magic4.SetCarrier(1, 1);  // sphere
        magic4.SetMagicSkill(3);  // 变快
        magic4.SetMagicSkill(3);  // 变快
        magic4.SetMagicSkill(3);  // 变快
        magic4.SetMagicSkill(2);  // 变强
        magic4.SetMagicSkill(2);  // 变强
        magic4.SetMagicSkill(2);  // 变强
        magic4.SetMagicSkill(8);  // 自动寻敌
        magic4.SetMagicSkill(9);  // 延时
        magic4.SetMagicSkill(9);  // 延时
        magic4.SetMagicSkill(9);  // 延时
        magic4.SetMagicSkill(10); // 发射
    }
    #endregion

    bool spelling = false;
    int magicID = 1;
    void UseMagic(Vector3 startPos, Vector3 shootDir, Vector3? hitPoint)
    #region
    {
        //    magicID = (magicID + 1) % 2;
        //    switch (magicID + 1)
        //    {
        //case 1:
        //    magic1.Use(startPos, shootDir, hitPoint);
        //    break;
        //case 2:
        //    magic2.Use(startPos, shootDir, hitPoint);
        //    break;
        //case 3:
        //    magic3.Use(startPos, shootDir, hitPoint);
        //    break;
        //case 4:
        //    magic4.Use(startPos, shootDir, hitPoint);
        //    break;

        //case 1:
        //    magic1.Use(startPos, shootDir, hitPoint);
        //    break;
        //case 2:
        //    magic4.Use(startPos, shootDir, hitPoint);
        //    break;
        //}
        magic4.Use(startPos, shootDir, hitPoint);
    }
    #endregion

    bool shooting = false;
    void ShootArrow()
    #region
    {
        Vector3 shootDir = Main.MainCamera.transform.forward;
        Vector3? hitPoint = GetClosestRayHitPoint();
        if (hitPoint.IsNotNull())
        {
            shootDir = (Vector3)hitPoint - magicShootPoint.transform.position;
        }

        GameObject go = LoadTool.LoadCarrier(CarrierManager.GetCarrierInfoByID(1).carrierPath);
        CarrierEntity carrier = go.GetOrAddComponent<CarrierEntity>();

        AudioManager.PlayOneShot("arrow_shoot");
        carrier.Init(CarrierManager.GetCarrierInfoByID(1),
                magicShootPoint.transform.position,
                shootDir,
                null,
                go,
                this,
                10,
                10,
                2,
                1,
                false,
                true);
    }
    #endregion

    void EventInit()                     // 接触地面刷新重力方向上的速度，的地面检测事件注册
    #region
    {
        checkGround.Add((state) =>
        {
            if(state == true)
            {
                // 刷新垂直方向上的速度
                ResetDownSpeed();
                IsJump = false;
            }
        });
    }
    #endregion

    void SkillInit()
    #region
    {
        skillFsm = new SkillFsm();
        skillFsm.Init(this, new List<FsmState<SkillFsm>>()
        {
            new Dash(skillFsm),
            new NoSkill(skillFsm),
        });
    }
    #endregion

    public void RefreshCheckGround()     // 重刷是否在地面
    #region
    {
        checkGround.ClearUnActive();
    }
    #endregion


    float horizontal;
    float vertical;
    bool move;
    Vector3 dir;
    float moveCount = 0f;
    void MoveTick()
    #region
    {
        if (Main.MainCameraCtrl.IsNull()) { return; }

        HorizontalMove();    // 平面上的移动
        VerticalMove();      // 模拟重力移动
    }
    #endregion

    
    void HorizontalMove()                // VerticalMove 在父类UnitEntity里
    #region
    {
        move = false;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            move = true;
        }
        if (Mathf.Abs(vertical) > 0.01f)
        {
            move = true;
        }

        IsMoving = move;
        if (move)
        {
            dir = (vertical * Main.MainCameraCtrl.Forward + horizontal * Main.MainCameraCtrl.Right).normalized;
            characterController.Move(dir * speed * Time.deltaTime);
            targetYEuler = QUtil.GetDegY(dir);
            MoveDistanceCheck((dir * speed * Time.deltaTime).magnitude);
        }
    }

    void MoveDistanceCheck(float distance)
    {
        // Move Tick
        moveCount += distance;
        if (moveCount > moveDistanceTick)
        {
            if (MoveTickAction.IsNotNull())
            {
                MoveTickAction.Invoke(moveCount, transform.position);
            }
            moveCount = 0;
        }
    }
    #endregion


    float targetYEuler = 0f;
    float nowYEuler = 0f;
    float rotateCoeff = 20f;
    void RotateTick()
    #region
    {
        if(CameraView.ThirdPerson == Main.MainCameraCtrl.CameraView)
        {
            if (Mathf.Abs(targetYEuler - this.transform.eulerAngles.y) < 0.1f)
            {
                return;
            }
            nowYEuler = Mathf.LerpAngle(this.transform.eulerAngles.y, targetYEuler, rotateCoeff * Time.deltaTime);
            this.transform.eulerAngles = new Vector3(0, nowYEuler, 0);
        }
        else if(CameraView.OverShoulder == Main.MainCameraCtrl.CameraView)
        {
            targetYEuler = Main.MainCameraCtrl.transform.eulerAngles.y;
            this.transform.eulerAngles = new Vector3(0, targetYEuler, 0);
        }
    }
    #endregion

    public override void Destroy()
    #region
    {
        base.Destroy();
        skillFsm.Destroy();
        GameObject.Destroy(this.gameObject);
    }
    #endregion

}
