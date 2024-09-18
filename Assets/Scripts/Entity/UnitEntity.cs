using Constant;
using System;
using UnityEngine;

namespace QEntity
{
    public enum Camp
    {
        Camp0,
        Camp1,
        Camp2,
    }

    public enum Profession
    {
        Wizard,           // 法师
        Fighter,          // 近战
        Shooter,          // 射手
    }

    public class UnitEntity : MonoBehaviour
    {
        public Camp camp;
        public virtual Profession Profession
        {
            get;
            set;
        }

        Vector3 center;
        float holdRange;
        [NonSerialized]
        public bool noHurt = false;
        public bool isDead = false;

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
        protected Animator animator;
        public CharacterController characterController;

        protected HUDEntity hud;
        // 这个detect主要是对攻击对象的检测，如果有其他检测，在那个类里另加，不要用这个
        protected DetectHelper detect;
        private QuickTimer quickTimer;


        public float gravity = 78f;

        public virtual bool IsJump
        {
            get;
            set;
        }

        public virtual bool IsGrounded
        {
            get;
        }

        public virtual float JumpSpeed
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
            set { transform.position = value; }
            get { return transform.position; }
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


        Vector3 down;

        protected void SetJumpSpeed()
        {
            down.y = JumpSpeed;
        }

        protected void ResetDownSpeed()
        {
            down.y = -20;
        }

        protected void VerticalMove()
        #region
        {
            // 模拟重力
            if (IsJump || !IsGrounded)
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

        public void SetControl(bool state)   // 设置是否由自主控制移动，区分技能移动
        #region
        {
            banControl = !state;
        }
        #endregion

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
            animator.CrossFade(animName, fadeTime);
            if (callback != null)
            {
                float time = QUtil.GetAnimationClip(animator, animName).length;
                TimerManager.Add(time, callback);
            }
        }

        public void PlayAnim(string animName, Action callback = null)
        {
            animator.CrossFade(animName, 0f);
            if (callback != null)
            {
                float time = QUtil.GetAnimationClip(animator, animName).length;
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
