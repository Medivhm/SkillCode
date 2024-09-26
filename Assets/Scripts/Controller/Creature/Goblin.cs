using Constant;
using QEntity;
using UnityEngine;

namespace Creature
{
    public class Goblin : CreatureEntity
    {
        public CheckGround checkGround;

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
            gameObject.name = "兽人";
            camp = new Camp { CampID = 1 };

            basicAttr = new BasicAttr(CreatureConstant.DefaultEntityAttr);
            closeAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            farAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            magicAttackAttr = new AttackAttr(CreatureConstant.DefaultAttackAttr);
            BloodMax = 120f;
            Blood = 100f;
            Name = name;
            hud = Ctrl.AddHUD(this.transform, string.Format("[{0}] {1}", 1, "兽人"), 100f, 50f);


            MoveSpeed = 9;
        }

        public void SetNoMove(bool state)
        {
            noMove = state;
        }

        bool noMove = false;


        private void Update()
        {
            if (noMove) return;

            VerticalMove();

            SimpleAIMove();
        }

        bool moveDir = false;
        Vector3 dir1 = new Vector3(1, 0, 0);
        Vector3 dir2 = new Vector3(-1, 0, 0);
        float moveRoad = 0f;
        float moveMaxRoad = 40f;
        bool wait = false;
        float waitTime = 0f;
        float waitMaxTime = 3f;
        private void SimpleAIMove()
        {
            if (wait)
            {
                IsMoving = false;
                waitTime += Time.deltaTime;
                if(waitTime > waitMaxTime)
                {
                    wait = false;
                    waitTime = 0f;
                }
                return;
            }

            IsMoving = true;
            if (moveDir)
            {
                this.transform.eulerAngles = new Vector3(0, QUtil.GetDegY(dir1), 0);
                characterController.Move(dir1.normalized * Time.deltaTime * MoveSpeed);
                moveRoad += Time.deltaTime * MoveSpeed;
                if(Mathf.Abs(moveRoad) > moveMaxRoad)
                {
                    moveRoad = 0;
                    moveDir = !moveDir;
                    wait = true;
                }
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0, QUtil.GetDegY(dir2), 0);
                characterController.Move(dir2.normalized * Time.deltaTime * MoveSpeed);
                moveRoad += Time.deltaTime * MoveSpeed;
                if (Mathf.Abs(moveRoad) > moveMaxRoad)
                {
                    moveRoad = 0;
                    moveDir = !moveDir;
                    wait = true;
                }
            }
        }
    }
}
