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
    public float moveDistanceTick = 10f;      // �ƶ��೤������һ�ξ������Tick
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
        //hud = Ctrl.AddHUD(this.transform, string.Format("[{0}] {1}", 1, "�������"), 100f, 50f);
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

    
    void DetectInit()                    // ��ʼ��������������
    #region
    {
        detect = new DetectHelper();
        detect.Init(this, 25, "Unit");
        detect.AddTargetEnterEvent((GameObject go) =>
        {
            GUI.ActionInfoLog(string.Format("[{0}] �����ⷶΧ", go.name));
        }); 
        detect.AddTargetLeaveEvent((GameObject go) =>
        {
            GUI.ActionInfoLog(string.Format("[{0}] �뿪", go.name));
        });
    }
    #endregion

    void KeyboardPressInit()             // ��ݼ���ʼ��
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
                SetJumpSpeed();           // ������Ծ�ٶ�
            }
        });
        Ctrl.SetQuickKey(KeyCode.R, () =>
        {
            Position = new Vector3(0, 10, 0);
        });
    }
    #endregion

    private void OnDrawGizmos()          // ����ս���������
    #region
    {
        if (Profession.Fighter == this.Profession)
        {
            Gizmos.color = Color.red;

            // ����������ϵ�л���һ������ OverlapBox ���߿����
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + this.transform.forward * 1.5f + this.transform.up * 2, this.transform.rotation, Vector3.one);
            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1.5f, 1.5f, 1.5f) * 2); // ʹ�� WireCube �����߿����
        }
    }
    #endregion

    void MouseClickInit()                // �������ʼ��
    #region
    {
        // RaycastHit[]
        MainMouseController.Instance.AddMouseLeftDown(() =>
        {
            if (UIManager.HasUI()) return;
            // ��ʦ
            if (Profession.Wizard == this.Profession)
            {
                if (spelling) return;

                spelling = true;
                Main.MagicProgressbar.Show();
                Main.MagicProgressbar.StartProgressBar(0.5f);
            }
            // սʿ
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
                GUI.ActionInfoLog(string.Format("�������� {0} ����", (units.Count - 1).ToString()));
            }
            // ����
            else if(Profession.Shooter == this.Profession)
            {
                if (shooting) return;

                shooting = true;
                Main.MainCameraCtrl.SetCameraView(CameraView.OverShoulder);
            }
        });


        MainMouseController.Instance.AddMouseLeftUp(() =>
        {
            // ��ʦ
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
            // սʿ
            else if (Profession.Fighter == this.Profession)
            {
            }
            // ����
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

    
    public Vector3? GetClosestRayHitPoint()        // ��ȡ��������ĵ�
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
        magic1.SetCarrier(3, 1);  // Բ��
        magic1.SetMagicSkill(1);  // ���
        magic1.SetMagicSkill(2);  // ��ǿ
        magic1.SetMagicSkill(7);  // ����
        magic1.SetMagicSkill(8);  // �Զ�Ѱ��
        magic1.SetMagicSkill(2);  // ��ǿ
        magic1.SetMagicSkill(2);  // ��ǿ
        magic1.SetMagicSkill(2);  // ��ǿ


        magic2 = Magic.Magic.GetAMagic(this);
        magic2.SetCarrier(2, 1);  // sphere
        magic2.SetMagicSkill(3);  // ���
        magic2.SetMagicSkill(3);  // ���
        magic2.SetMagicSkill(3);  // ���
        magic2.SetMagicSkill(3);  // ���
        magic2.SetMagicSkill(2);  // ��ǿ
        magic2.SetMagicSkill(2);  // ��ǿ
        magic2.SetMagicSkill(2);  // ��ǿ
        magic2.SetMagicSkill(2);  // ��ǿ


        magic3 = Magic.Magic.GetAMagic(this);
        magic3.SetCarrier(1, 1);  // cube
        magic3.SetMagicSkill(4);  // 2����
        magic3.SetMagicSkill(5);  // 3����
        

        magic4 = Magic.Magic.GetAMagic(this);
        magic4.SetCarrier(1, 1);  // sphere
        magic4.SetMagicSkill(3);  // ���
        magic4.SetMagicSkill(3);  // ���
        magic4.SetMagicSkill(3);  // ���
        magic4.SetMagicSkill(2);  // ��ǿ
        magic4.SetMagicSkill(2);  // ��ǿ
        magic4.SetMagicSkill(2);  // ��ǿ
        magic4.SetMagicSkill(8);  // �Զ�Ѱ��
        magic4.SetMagicSkill(9);  // ��ʱ
        magic4.SetMagicSkill(9);  // ��ʱ
        magic4.SetMagicSkill(9);  // ��ʱ
        magic4.SetMagicSkill(10); // ����
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

    void EventInit()                     // �Ӵ�����ˢ�����������ϵ��ٶȣ��ĵ������¼�ע��
    #region
    {
        checkGround.Add((state) =>
        {
            if(state == true)
            {
                // ˢ�´�ֱ�����ϵ��ٶ�
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

    public void RefreshCheckGround()     // ��ˢ�Ƿ��ڵ���
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

        HorizontalMove();    // ƽ���ϵ��ƶ�
        VerticalMove();      // ģ�������ƶ�
    }
    #endregion

    
    void HorizontalMove()                // VerticalMove �ڸ���UnitEntity��
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
