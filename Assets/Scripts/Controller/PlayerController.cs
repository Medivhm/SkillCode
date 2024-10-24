using QEntity;
using Manager;
using UnityEngine;
using System;
using System.Collections.Generic;
using Fsm;
using Constant;
using Tools;



public partial class PlayerController : PlayerEntity
{
    [HideInInspector] public float shiftTime = 0.2f;
    float shiftPressTime = 0f;
    public bool shiftPress = false;
    public bool leftCtrlPress = false;
    public UnitAct UnitAct
    {
        get
        {
            return unitAct;
        }
        set
        {
            if (unitAct == value) return;
            unitAct = value;

            if (unitAct == UnitAct.Squat)
            {
                SetColliderSquat();
                SetWalkStairsState(true);
            }
            else if(unitAct == UnitAct.Stand)
            {
                SetColliderStand();
                SetWalkStairsState(true);
            }
            else if(unitAct == UnitAct.Skill)
            {
                // skill �Լ�����Collider
                SetWalkStairsState(false);
            }
        }
    }

    public void SetWalkStairsState(bool state)
    {
        if (state)
        {
            maxStableSlopeAngle = 60f;
            MaxStepHeight = 1.8f;
        }
        else
        {
            maxStableSlopeAngle = 0f;
            MaxStepHeight = 0f;
        }
    }

    public void SetColliderStand()
    {
        CapsuleRadius = 0.52f;
        CapsuleHeight = 3.49f;
        CapsuleYOffset = 1.8f;
        QCC.SetCapsuleDimensions(CapsuleRadius, CapsuleHeight, CapsuleYOffset);
    }

    public void SetColliderSquat()
    {
        CapsuleRadius = 0.71f;
        CapsuleHeight = 2.19f;
        CapsuleYOffset = 1.08f;
        QCC.SetCapsuleDimensions(CapsuleRadius, CapsuleHeight, CapsuleYOffset);
    }

    public void LeftShiftPress()
    {
        shiftPress = true;
        groundMoveSpeed = shiftSpeed;
    }

    public void LeftShiftUp()
    {
        shiftPress = false;
        if(shiftPressTime < shiftTime)
        {
            skillFsm.ChangeState<Roll>();
        }
        shiftPressTime = 0f;
        groundMoveSpeed = commonGroundSpeed;
    }

    public void ShiftTick()
    {
        if (shiftPress && shiftPressTime < 10f)
        {
            shiftPressTime += Time.deltaTime;
        }
    }

    public void LeftControlPress()
    {
        leftCtrlPress = true;
        if(currentSpeed > 0.1f)
        {
            skillFsm.ChangeState<Sliding>();
        }
        else
        {
            UnitAct = UnitAct.Squat;
        }
    }

    public void LeftControlUp()
    {
        leftCtrlPress = false;
        groundMoveSpeed = commonGroundSpeed;
        if(UnitAct != UnitAct.Skill)
        {
            UnitAct = UnitAct.Stand;
        }
    }
}

public partial class PlayerController : PlayerEntity
{
    // ��������
    [HideInInspector] public float moveDistanceTick = 10f;      // �ƶ��೤������һ�ξ������Tick
    [HideInInspector] public float squatSpeed = 5f;
    [HideInInspector] public float shiftSpeed = 15f;
}

public partial class PlayerController : PlayerEntity   // QCC ���
{
    private void QCCInit()
    {
        SetColliderStand();
        SetGroundMoveSpeed(commonGroundSpeed);
        SetAirMoveSpeed(commonAirSpeed);
    }

    public void SetGroundMoveSpeed(float speed)
    {
        groundMoveSpeed = speed;
    }

    public void SetAirMoveSpeed(float speed)
    {
        airMoveSpeed = speed;
    }
}

public partial class PlayerController : PlayerEntity
{
    public Transform magicShootPoint;
    public Transform cameraFollowPoint;
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
            if(isMoving != value)
            {
                isMoving = value;
            }

            if (isMoving == true)
            {
                if (UnitAct.Squat == UnitAct)
                {
                    PlayAnim(Anim.SquatMoveAnim);
                }
                else
                {
                    PlayAnim(Anim.RunAnim);
                }
            }
            else
            {
                if (UnitAct.Squat == UnitAct)
                {
                    PlayAnim(Anim.SquatIdleAnim);
                }
                else
                {
                    PlayAnim(Anim.IdleAnim);
                }
            }
        }
    }

    public override bool IsGrounded
    {
        get
        {
            return QCC.Motor.GroundingStatus.IsStableOnGround;
        }
    }

    private void OnDestroy()
    {
        Main.MainPlayerCtrl = null;
    }

    void Start()
    {
        Main.MainPlayerCtrl = this;
        gameObject.name = string.Format("[{0}]", 0);
        //hud = Ctrl.AddHUD(this.transform, string.Format("[{0}] {1}", 1, "�������"), 100f, 50f);
        InitPropers();
        InitHUD();
        DetectInit();
        EventInit();
        KeyboardPressInit();
        SkillInit();
        MouseClickInit();
        QCCInit();

        MoveTickAction += ChunkManager.Instance.CheckPlayerPos;
        Test();

        camp = new Camp { CampID = 0 };
        UnitAct = UnitAct.Stand;
        Profession = Profession.Fighter;



        Main.MainCameraCtrl.SetFollowTransform(cameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        Main.MainCameraCtrl.IgnoredColliders.Clear();
        Main.MainCameraCtrl.IgnoredColliders.AddRange(this.QCC.GetComponentsInChildren<Collider>());
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
        ShiftTick();
    }

    void DetectInit()                    // ��ʼ��������������
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

    void KeyboardPressInit()             // ��ݼ���ʼ��
    {
        Ctrl.AddKeyPress(KeyCode.Space, () =>
        {
            if(UnitAct == UnitAct.Squat)
            {
                BigJump();
            }
            Jump();
        });
        Ctrl.AddKeyPress(KeyCode.R, () =>
        {
            Position = new Vector3(0, 10, 0);
        });
        Ctrl.AddKeyPress(KeyCode.Q, () =>
        {
            AddVelocity(new Vector3(0, 50, 0));
        });

        Ctrl.AddKeyPress(KeyCode.LeftShift, LeftShiftPress);
        Ctrl.AddKeyPress(KeyCode.LeftControl, LeftControlPress);
        Ctrl.AddKeyUp(KeyCode.LeftShift, LeftShiftUp);
        Ctrl.AddKeyUp(KeyCode.LeftControl, LeftControlUp);
    }

    void MouseClickInit()                // �������ʼ��
    {
        // RaycastHit[]
        MainMouseController.Instance.AddMouseLeftDown(() =>
        {
            Item item = Main.HotBarUI.GetNowSelectedItem();
            if (item.IsNull()) return;
            if (UIManager.HasUI()) return;

            if (item is Weapon)
            {
                // ��ʦ
                //if (Profession.Wizard == this.Profession)
                if (item.Type == 2)
                {
                    if (spelling) return;

                    spelling = true;
                    Main.MagicProgressbar.Show();
                    Main.MagicProgressbar.StartProgressBar(0.5f);
                }
                // սʿ
                //else if (Profession.Fighter == this.Profession)
                else if (item.Type == 3)
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
                //else if (Profession.Shooter == this.Profession)
                else if (item.Type == 1)
                {
                    if (shooting) return;

                    shooting = true;
                    Main.MainCameraCtrl.SetCameraView(CameraView.OverShoulder);
                }
            }
            else if (item is Prop)
            {
                HotBar.Instance.UseItem(item);
            }
        });


        MainMouseController.Instance.AddMouseLeftUp(() =>
        {
            Item item = Main.HotBarUI.GetNowSelectedItem();
            if (item.IsNull()) return;
            if (UIManager.HasUI()) return;

            // ��ʦ
            //if (Profession.Wizard == this.Profession)
            if (item is Weapon)
            {
                if (item.Type == 2)
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
                //else if (Profession.Fighter == this.Profession)
                else if (item.Type == 3)
                {
                }
                // ����
                //else if (Profession.Shooter == this.Profession)
                else if (item.Type == 1)
                {
                    if (!UIManager.HasUI())
                    {
                        ShootArrow();
                    }
                    Main.MainCameraCtrl.SetCameraView(CameraView.ThirdPerson);
                    shooting = false;
                }
            }
        });
    }

    public void BigJump()                // ����
    {
        if (isMoving)
        {
            AddVelocity((Dir + Vector3.up).normalized * 40f);
        }
        else
        {
            AddVelocity(Vector3.up * 40f);
        }
    }                          

    private void OnDrawGizmos()          // ����ս���������
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

    public Vector3? GetClosestRayHitPoint()        // ��ȡ��������ĵ�
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

    bool shooting = false;
    void ShootArrow()
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

    void EventInit()
    {

    }

    void SkillInit()
    {
        skillFsm = new SkillFsm();
        skillFsm.Init(this, new List<FsmState<SkillFsm>>()
        {
            new Dash(skillFsm),
            new Roll(skillFsm),
            new Sliding(skillFsm),
            new NoSkill(skillFsm),
        });
    }

    float horizontal;
    float vertical;
    bool move;
    Vector3 dir;
    float moveCount = 0f;
    void MoveTick()
    {
        if (Main.MainCameraCtrl.IsNull()) return;
        if (UIManager.HasUI()) return;

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
        Vector3 dirtyMoveDir = vertical * Main.MainCameraCtrl.Forward + horizontal * Main.MainCameraCtrl.Right;
        dir = dirtyMoveDir.normalized;
        Move(Vector3.ClampMagnitude(dirtyMoveDir, 1f));
        if (move)
        {
            targetYEuler = QUtil.GetDegY(dir);
        }
        MoveDistanceCheck(currentSpeed * Time.deltaTime);
    }

    void MoveDistanceCheck(float distance)
    {
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


    float targetYEuler = 0f;
    float nowYEuler = 0f;
    float rotateCoeff = 20f;
    void RotateTick()
    {
        if(CameraView.ThirdPerson == Main.MainCameraCtrl.CameraView)
        {
            if (Mathf.Abs(targetYEuler - this.transform.eulerAngles.y) < 0.1f)
            {
                return;
            }
            nowYEuler = Mathf.LerpAngle(this.transform.eulerAngles.y, targetYEuler, rotateCoeff * Time.deltaTime);
            //this.transform.eulerAngles = new Vector3(0, nowYEuler, 0);
            QCC.SetRotation(Quaternion.Euler(0, nowYEuler, 0));
        }
        else if(CameraView.OverShoulder == Main.MainCameraCtrl.CameraView)
        {
            targetYEuler = Main.MainCameraCtrl.transform.eulerAngles.y;
            QCC.SetRotation(Quaternion.Euler(0, targetYEuler, 0));
            //this.transform.eulerAngles = new Vector3(0, targetYEuler, 0);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            //// ����ͷ���ĸ���Ȩ��
            //animator.SetLookAtWeight(lookWeight);

            //// ����ͷ�������Ŀ��λ��
            //if (lookTarget != null)
            //{
            //    animator.SetLookAtPosition(lookTarget.position);
            //}
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        skillFsm.Destroy();
        GameObject.Destroy(this.gameObject);
    }
}
