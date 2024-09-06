using QEntity;
using Manager;
using UnityEngine;
using System;
using System.Collections.Generic;
using Fsm;
using System.Collections;

public enum MoveType
{
    SkillMove,
    UserMove,
    AutoMove,
}

public class PlayerController : PlayerEntity
{
    public CheckGround checkGround;
    public CharacterController characterController;
    public Transform magicShootPoint;

    public float speed = 10f;
    public float jumpSpeed = 26f;
    public float gravity = 78f;
    public float moveDistanceTick = 10f;      // 移动多长距离做一次距离相关Tick
    public SkillFsm skillFsm;

    public Action<float, Vector3> MoveTickAction;    // arg1:move distance, arg2:position

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
                Play("Run", 0.01f);
            }
            else
            {
                Play("Idle", 0.01f);
            }
        }
    }

    public bool IsGrounded
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

    void Start()
    {
        Main.MainPlayerCtrl = this;
        camp = Camp.Camp0;
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
    }

    void Update()
    {
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

    void DetectInit()
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
        Ctrl.SetQuickKey(KeyCode.Alpha1, () =>
        {
            Play("Dance1", 0.1f);
        });
        Ctrl.SetQuickKey(KeyCode.Alpha2, () =>
        {
            Play("Dance2", 0.1f);
        });
        Ctrl.SetQuickKey(KeyCode.Alpha3, () =>
        {
            Play("Dance3", 0.1f);
        });
        Ctrl.SetQuickKey(KeyCode.Space, () =>
        {
            if (IsGrounded)
            {
                isJump = true;
                down.y = jumpSpeed;
            }
        });
        Ctrl.SetQuickKey(KeyCode.R, () =>
        {
            StartCoroutine(ResetPosition());
        });
    }
    #endregion

    void MouseClickInit()                // 鼠标点击初始化
    #region
    {
        // RaycastHit[]
        MainMouseController.Instance.AddMouseLeftDown(() =>
        {
            if (spelling) return;
            if (UIManager.HasUI()) return;

            spelling = true;
            Main.MagicProgressbar.Show();
            Main.MagicProgressbar.StartProgressBar(0.5f);
        });


        MainMouseController.Instance.AddMouseLeftUp(() =>
        {
            spelling = false;
            if (Main.MagicProgressbar.ChargeOver)
            {
                Vector3 shootDir = Main.MainCamera.transform.forward;
                var infos = MainMouseController.Instance.GetCenterScreenRayHit();
                for (int i = infos.Length - 1; i >= 0; i--)
                {
                    if (infos[i].collider.gameObject.Equals(this.gameObject))
                        continue;

                    //info.point;
                    shootDir = infos[i].point - magicShootPoint.position;
                    break;
                }
                UseMagic(magicShootPoint.position, shootDir);
            }
            Main.MagicProgressbar.Hide();
            Main.MagicProgressbar.ResetData();
        });
    }
    #endregion


    Magic.Magic magic1;
    Magic.Magic magic2;
    Magic.Magic magic3;
    void Test()
    {
        magic1 = Magic.Magic.GetAMagic(this);
        magic1.SetCarrier(1, 1);  // cube
        magic1.SetMagicSkill(1);  // 变大
        magic1.SetMagicSkill(1);  // 变大
        magic1.SetMagicSkill(2);  // 变强
        magic1.SetMagicSkill(7);  // 定点
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
        magic3.SetMagicSkill(6);  // 追踪
    }

    bool spelling = false;
    void UseMagic(Vector3 startPos, Vector3 shootDir)
    {
        switch (Util.GetRandom(1, 3))
        {
            case 1:
                magic1.Use(startPos, shootDir);
                break;
            case 2:
                magic2.Use(startPos, shootDir);
                break;
            case 3:
                magic3.Use(startPos, shootDir);
                break;
        }
    }

    IEnumerator ResetPosition()
    {
        SetControl(false);
        yield return null;
        transform.position = new Vector3(0, 10, 0);
        yield return null;
        SetControl(true);
    }

    void EventInit()                     // 接触地面刷新重力方向上的速度，的地面检测事件注册
    #region
    {
        checkGround.Add((state) =>
        {
            if(state == true)
            {
                // 刷新垂直方向上的速度
                down.y = -20;
                isJump = false;
            }
        });
    }
    public void ResetDown()
    {
        down.y = -20;
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
    Vector3 down;
    bool isJump = false;
    float moveCount = 0f;
    void MoveTick()
    #region
    {
        if (Main.MainCameraCtrl.IsNull()) { return; }

        HorizontalMove();    // 平面上的移动
        VerticalMove();      // 模拟重力移动
    }
    #endregion

    void HorizontalMove()
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
            targetYEuler = Util.GetDegY(dir);
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

    void VerticalMove()
    #region
    {
        // 模拟重力
        if (isJump || !IsGrounded)
        {
            down.y -= gravity * Time.deltaTime;
            characterController.Move(down * Time.deltaTime);
        }
        else
        {
            characterController.Move(down * Time.deltaTime);
        }
    }
    #endregion

    float targetYEuler = 0f;
    float nowYEuler = 0f;
    float rotateCoeff = 20f;
    void RotateTick()
    #region
    {
        if (Mathf.Abs(targetYEuler - this.transform.eulerAngles.y) < 0.1f)
        {
            return;
        }
        nowYEuler = Mathf.LerpAngle(this.transform.eulerAngles.y, targetYEuler, rotateCoeff * Time.deltaTime);
        this.transform.eulerAngles = new Vector3(0, nowYEuler, 0);
    }
    #endregion

    void Play(string animName, float fadeTime)
    #region
    {
        animator.CrossFade(animName, fadeTime);
    }

    void Play(string animName)
    {
        animator.Play(animName);
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

    private void OnDestroy()
    {
        Main.MainPlayerCtrl = null;
    }
}
