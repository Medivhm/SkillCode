using Constant;
using System;
using UnityEngine;

namespace QEntity
{
    public enum Camp
    {
        Player,
        Enemy,
        Mid,
    }

    public class UnitEntity : MonoBehaviour
    {
        Vector3 center;
        float holdRange;
        [NonSerialized]
        public bool noHurt = false;
        public bool isDead = false;

        public BasicAttr basicAttr;
        public AttackAttr closeAttackAttr;
        public AttackAttr farAttackAttr;
        public AttackAttr magicAttackAttr;
        public Action BloodChange;
        public Action<float, char> GetHurtAction;
        public bool banControl = false;
        public Vector3 Dir => this.transform.forward;

        [SerializeField]
        protected Animator animator;

        protected HUDEntity hud;
        // 这个detect主要是对攻击对象的检测，如果有其他检测，在那个类里另加，不要用这个
        //protected DetectHelper detect;
        private QuickTimer quickTimer;


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

        public void PlayAnim(string animName, Action callback = null)
        {
            if (callback != null)
            {
                float time = Util.GetAnimationClip(animator, animName).length;
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
