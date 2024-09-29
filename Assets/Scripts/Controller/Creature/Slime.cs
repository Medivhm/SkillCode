using Constant;
using QEntity;
using UnityEngine;

namespace Creature
{
    public class Slime : CreatureEntity
    {
        public CheckGround checkGround;
        public Collider OutCollider;          // 如果是character controller的胶囊体碰撞，会走到头顶上，但是同高度的非弧形碰撞体不会
        public override float OutColliderRadius => OutCollider.bounds.size.x / 2;

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

        public float jumpSpeed = 26f;
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

        private bool isMoving;
        public bool IsMoving
        {
            get { return isMoving; }
            set
            {
                if (isMoving == value)
                {
                    return;
                }

                isMoving = value;
                if (isMoving == true)
                {
                    PlayAnim("Run", 0.01f);
                }
                else
                {
                    PlayAnim("Idle", 0.01f);
                }
            }
        }

        private void Start()
        {
            gameObject.name = "史莱姆";
            camp = new Camp { CampID = 2 };

            basicAttr = new BasicAttr(CreatureConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            BloodMax = 60f;
            Blood = 50f;
            Name = name;
            hud = Ctrl.AddHUD(this.transform, string.Format("[{0}] {1}", 1, "史莱姆"), BloodMax, Blood);

            MoveSpeed = 9;
            DetectInit();
        }

        UnitEntity Target
        {
            get
            {
                if (simpleDetect.IsNotNull())
                {
                    return simpleDetect.Target;
                }
                return null;
            }
        }

        CommonDetectHelper simpleDetect;
        void DetectInit()                    // 初始化距离内人物检测
        #region
        {
            simpleDetect = new CommonDetectHelper();
            simpleDetect.Init(this.gameObject, 10, "Unit", camp);
        }
        #endregion


        public void SetNoMove(bool state)
        {
            noMove = state;
        }

        bool noMove = false;
        protected override void Update()
        {
            base.Update();
            AttackColdTick();

            if (noMove) return;
            VerticalMove();
            SimpleAIMove();
        }

        Vector3 moveDir;
        private void SimpleAIMove()
        {
            if (Target.IsNotNull())
            {
                if (Vector3.Distance(Target.transform.position, Position) < (attackRadius + Target.OutColliderRadius + OutColliderRadius))
                {
                    Attack(Vector3.one);
                    return;
                }

                moveDir = Target.transform.position - this.transform.position;
                this.transform.eulerAngles = new Vector3(0, QUtil.GetDegY(moveDir), 0);
                characterController.Move(moveDir.normalized * Time.deltaTime * MoveSpeed);
            }
        }

        private void AttackColdTick()
        {
            if (!canAttack)
            {
                timeCount -= Time.deltaTime;
                if(timeCount < 0)
                {
                    canAttack = true;
                }
            }
        }


        bool canAttack = true;
        float attackCold = 1f;
        float timeCount = 0f;
        bool isAttacking = false;
        public override bool Attack(Vector3 _)
        {
            if (isAttacking) return false;

            if(Target.IsNotNull() && canAttack)
            {
                isAttacking = true;
                PlayAnim("Attack", 0.1f, () =>
                {
                    if (isDead) return;
                    PlayAnim("Idle", 0.1f);
                    canAttack = false;
                    isAttacking = false;
                    timeCount = attackCold;
                });
                Target.BeAttack((Target.Position - Position).normalized);
                Target.GetHurt(5f);
            }
            return false;
        }

        public override void Dead()
        {
            base.Dead();
            Item item = Ctrl.CreateItem(1);
            item.Owner = Main.MainPlayerCtrl;
            Ctrl.DropItem(item, this.transform.position);
            Destroy();
            GameObject.Destroy(this.gameObject);
        }
    }
}
