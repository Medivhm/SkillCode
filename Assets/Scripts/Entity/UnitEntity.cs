using KinematicCharacterController.Walkthrough.AddingImpulses;
using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public enum UnitAct
{
    Stand,
    Squat,
    Skill,
}

namespace QEntity
{
    public struct Camp
    {
        public int CampID;
    }

    public enum Profession
    {
        Wizard,           // 法师
        Fighter,          // 近战
        Shooter,          // 射手
    }

    [RequireComponent(typeof(QCharacterController))]
    public class UnitEntity : MonoBehaviour
    {
        [HideInInspector] public float groundMoveSpeed = 12;
        [HideInInspector] public float airMoveSpeed = 12;
        [HideInInspector] public float commonGroundSpeed = 12f;
        [HideInInspector] public float commonAirSpeed = 12f;
        [HideInInspector] public float jumpSpeed = 30f;
        [HideInInspector] public float movementSharpness = 15f;
        [HideInInspector] public float drag = 0.1f;
        [HideInInspector] public float airAccelerationSpeed = 1f;
        [HideInInspector] public float maxStableSlopeAngle = 60;
        [HideInInspector] public float MaxStepHeight = 1.8f;
        [HideInInspector] public Vector3 gravity = new Vector3(0, -78f, 0);

        [SerializeField] public float CapsuleRadius  = 0.5f;
        [SerializeField] public float CapsuleHeight  = 2f;
        [SerializeField] public float CapsuleYOffset = 1f;

        public float currentSpeed => QCC.GetCurrentSpeed();

        public float attackRadius = 2f;
        public Camp camp;
        public UnitAct unitAct;
        public virtual Profession Profession
        {
            get;
            set;
        }
        public virtual float OutColliderRadius
        {
            get => QCC.Capsule.radius;
        }

        Vector3 center;
        float holdRange;
        [NonSerialized]
        public bool noHurt = false;
        public bool isDead = false;
        public QCharacterController QCC;

        // 实际魔法弹自瞄位置
        public Transform hitPosition;
        public BasicAttr basicAttr;
        public AttackAttr closeAttackAttr;
        public AttackAttr farAttackAttr;
        public AttackAttr magicAttackAttr;
        public Action BloodChange;
        public Action<float, char> GetHurtAction;
        public bool banControl = false;
        public Vector3 Dir => this.transform.forward;
        //[HideInInspector]
        public UnitEntity lastAttack;

        [SerializeField]
        public Animator animator;

        protected HUDEntity hud;
        // 这个detect主要是对攻击对象的检测，如果有其他检测，在那个类里另加，不要用这个
        protected DetectHelper detect;
        private QuickTimer quickTimer;
        public Action DeadEvent;

        public virtual bool IsJump
        {
            get;
            set;
        }

        public virtual bool IsGrounded
        {
            get;
        }

        public float HoldRange
        {
            get { return holdRange; }
            set { holdRange = value; }
        }

        public Vector3 Center
        {
            get { return center; }
            set { center = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public float Blood
        {
            get { return basicAttr.blood; }
            set
            {
                if (value < 0.01f)
                {
                    value = 0.00f;
                    isDead = true;
                }
                else
                {
                    isDead = false;
                }

                if (value > BloodMax)
                {
                    value = BloodMax;
                }
                basicAttr.blood = value;
                if (hud)
                {
                    hud.Refresh(value);
                }
                if (BloodChange != null)
                {
                    BloodChange.Invoke();
                }
                if (isDead)
                {
                    Dead();
                }
            }
        }

        public float BloodRecover
        {
            set { basicAttr.bloodRecover = value; }
            get { return basicAttr.bloodRecover; }
        }

        public Vector3 Position
        {
            set { QCC.SetPosition(value); }
            get => transform.position;
        }  

        public Vector3 LocalPosition
        {
            set { transform.localPosition = value; }
            get { return transform.localPosition; }
        }

        public float BloodMax
        {
            set { basicAttr.bloodMax = value; }
            get { return basicAttr.bloodMax; }
        }

        public float MoveSpeed
        {
            set { basicAttr.moveSpeed = value; }
            get { return basicAttr.moveSpeed; }
        }

        public virtual void Awake()
        {
            quickTimer = new QuickTimer();
            quickTimer.Init();
        }

        public void OnValidate()
        {
            QCC.Motor.ValidateData();
        }

        public void UpdateCapsule()
        {
            QCC.Capsule.radius = CapsuleRadius;
            QCC.Capsule.height = Mathf.Clamp(CapsuleHeight, CapsuleRadius * 2f, CapsuleHeight);
            QCC.Capsule.center = new Vector3(0f, CapsuleYOffset, 0f);
        }

        public void SetJumpSpeed(float jumpSpeed)
        {
            this.jumpSpeed = jumpSpeed;
        }

        public void AddVelocity(Vector3 velocity)
        {
            QCC.AddVelocity(velocity);
        }

        //public void AddForce(Vector3 force)
        //{
        //    QCC.AddForce(force);
        //}

        public void Move(Vector3 moveDir)
        {
            QCC.Move(moveDir);
        }

        public void MoveToPosition(Vector3 position)
        {
            QCC.MoveToPosition(position);
        }

        public void MoveTo(Vector3 dist)
        {
            MoveToPosition(Position + dist);
        }

        public void Jump()
        {
            QCC.Jump();
        }


        protected virtual void Update()
        {

        }

        public void SetControl(bool state)   // 设置是否由自主控制移动，区分技能移动
        {
            banControl = !state;
        }

        public void GetHurt(float damage)
        {
            if (noHurt) return;

            Blood -= damage;
            if (hud.IsNotNull())
            {
                hud.JumpNum("-" + damage.ToString(), Color.red);
            }
            if (GetHurtAction.IsNotNull())
            {
                GetHurtAction.Invoke(damage, '-');
            }
        }

        public UnitEntity GetClosestOtherCamp()
        {
            return detect.GetClosestOtherCamp();
        }

        public void PlayAnim(string animName, float fadeTime, Action callback = null)
        {
            if (animName == nowAnimPlay) return;
            if (unitAct == UnitAct.Skill) return;

            nowAnimPlay = animName;
            animator.CrossFade(animName, fadeTime);
            if (callback != null)
            {
                float time = QUtil.GetAnimationClip(animator, animName).length;
                TimerManager.Add(time, callback);
            }
        }

        string nowAnimPlay;
        public void PlayAnim(string animName, Action callback = null)
        {
            if (animName == nowAnimPlay) return;
            if (unitAct == UnitAct.Skill) return;

            nowAnimPlay = animName;
            //Debug.Log(animName);
            float time = QUtil.GetAnimationClip(animator, animName).length;
            float fadeTime = time / 10;
            fadeTime = fadeTime > 0.1f ? 0.1f : fadeTime;

            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float remainingTime = stateInfo.length * (1f - (stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime)));
            if (remainingTime < 0.1f)
            {
                fadeTime = remainingTime;
            }

            animator.CrossFade(animName, fadeTime);

            if (callback != null)
            {
                TimerManager.Add(time, callback);
            }
        }

        public virtual bool Attack(Vector3 hitDir)
        {
            return false;
        }

        public virtual void Dead()
        {
            isDead = true;
            if (DeadEvent.IsNotNull())
            {
                DeadEvent.Invoke();
            }
        }

        public virtual void Revive()
        {
            isDead = false;
        }

        public virtual void DestroyHUD()
        {
            hud.Destroy();
        }

        public void DestroyTimers()
        {
            quickTimer.DestroyTimers();
        }

        public int AddTimer(float time, Action action)
        {
            return quickTimer.AddTimer(time, action);
        }

        public void RemoveTimer(int timerID)
        {
            quickTimer.RemoveTimer(timerID);
        }

        public void Speak(string content)
        {
            if (hud.IsNotNull())
            {
                hud.Speak(content);
            }
        }

        public void Speak(string content, float time)
        {
            if (hud.IsNotNull())
            {
                hud.Speak(content, time);
            }
        }

        public virtual void Destroy()
        {
            DestroyHUD();
            DestroyTimers();
        }
    }
}
