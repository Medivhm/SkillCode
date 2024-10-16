﻿using KinematicCharacterController.Walkthrough.AddingImpulses;
using System;
using UnityEditor;
using UnityEngine;

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
        public float attackRadius = 2f;
        public Camp camp;
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
        protected Animator animator;

        protected HUDEntity hud;
        // 这个detect主要是对攻击对象的检测，如果有其他检测，在那个类里另加，不要用这个
        protected DetectHelper detect;
        private QuickTimer quickTimer;
        public Action DeadEvent;

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


        Vector3 down;

        protected void SetJumpSpeed()
        {
            down.y = JumpSpeed;
        }

        protected void ResetDownSpeed()
        {
            down.y = -20;
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

        public void Jump()
        {
            QCC.Jump();
        }

        protected virtual void Update()
        {

        }

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
            float time = QUtil.GetAnimationClip(animator, animName).length;
            float fadeTime = time / 10;
            fadeTime = fadeTime > 0.1f ? 0.1f : fadeTime;

            DebugTool.Log(fadeTime + "AAAA");
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float remainingTime = stateInfo.length * (1f - (stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime)));
            if(remainingTime < 0.1f)
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
