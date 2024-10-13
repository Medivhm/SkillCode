using Constant;
using Info;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace QEntity
{
    public partial class CarrierEntity : MonoBehaviour
    {
        public float speed;
        // 生命周期，填null为永远存在，不通过定时器销毁
        private float? lifeTime = null;
        // 发射方向
        private Vector3 dir;
        // 自动瞄准目标，有则发射方向始终指向该目标，否则指定方向
        [SerializeField]
        private UnitEntity Target;
        private GameObject CarrierGo;
        private UnitEntity Owner;
        private QuickTimer quickTimer;

        public bool isDestroy = false;
        public bool isInit = false;

        public float power;
        public float speedMult;
        public float liveTimeMult;
        public float scale;

        public bool needRotate = false;
        public bool destroyWhenHurt = true;
        public float hurtColdTime = 1f;
        public bool autoSearch = false;
        public bool shootAtFirst;

        private void Awake()
        {
            hurtTimeCount = new Dictionary<GameObject, float>();
        }

        public void Init(CarrierInfo info,
            Vector3 pos,
            Vector3? dir,
            UnitEntity Target,
            GameObject CarrierGo,
            UnitEntity Owner,
            float power,
            float speedMult,
            float liveTimeMult,
            float scale, 
            bool autoSearch,
            bool shootAtFirst)
        {
            Init(info.speed, info.destroyWhenHurt, info.hurtColdTime, info.needRotate, pos, info.lifeTime, dir, Target, CarrierGo, Owner, power, speedMult, liveTimeMult, scale, autoSearch, shootAtFirst);
        }

        public void Init(float speed, 
            bool destroyWhenHurt, 
            float hurtColdTime, 
            bool needRotate,
            Vector3 pos, 
            float? lifeTime, 
            Vector3? dir,
            UnitEntity Target, 
            GameObject CarrierGo, 
            UnitEntity Owner, 
            float power, 
            float speedMult,
            float liveTimeMult,
            float scale,
            bool autoSearch,
            bool shootAtFirst)
        {
            this.destroyWhenHurt = destroyWhenHurt;
            this.hurtColdTime = hurtColdTime;
            this.needRotate = needRotate;
            this.power = power;
            this.speedMult = speedMult;
            this.liveTimeMult = liveTimeMult;
            this.scale = scale;
            this.autoSearch = autoSearch;
            this.dir = (dir ?? Vector3.forward).normalized;
            this.Target = Target;
            this.CarrierGo = CarrierGo;
            this.Owner = Owner;
            this.transform.position = pos;
            this.transform.localEulerAngles = new Vector3(0, QUtil.GetDegY((Vector3)dir), 0);
            this.speed = speed;
            this.lifeTime = lifeTime;
            this.speed *= speedMult;
            this.lifeTime *= liveTimeMult;
            this.shootAtFirst = shootAtFirst;

            targetDir = this.dir;
            transform.SetParent(Main.SceneRoot);
            transform.localScale = new Vector3(this.scale, this.scale, this.scale);
            quickTimer = new QuickTimer();
            quickTimer.Init();
            if (lifeTime != null)
            {
                quickTimer.AddTimer((float)lifeTime, () =>
                {
                    Destroy();
                });
            }
            isInit = true;
            isDestroy = false;

            GiveItMagic();
        }


        CommonDetectHelper commonDetectHelper;
        void GiveItMagic()
        {
            if (this.autoSearch)
            {
                if(commonDetectHelper.IsNull())
                {
                    commonDetectHelper = new CommonDetectHelper();
                    commonDetectHelper.Init(gameObject, 40, TagConstant.Unit, Owner.camp);
                }
            }
            else
            {
                if (commonDetectHelper.IsNotNull())
                {
                    commonDetectHelper.Destroy();
                    commonDetectHelper = null;
                }
            }
        }

        public void Update()
        {
            if (!isInit) return;
            if (isDestroy) return;

            LifeTimeTick();
            HurtTimeTick();
            RefreshTarget();
            if (Target.IsNotNull())
            {
                // 处理弹体旋转
                CalcRotate();
                // 处理移动方向
                if (!IsTooCloseToTarget())
                {
                    CalcDir();
                }
                else
                {
                    return;
                }
                // 移动
                Move();
            }
            // 没有目标时，只有给了[发射]tag，才能动
            if (shootAtFirst)
            {
                Move();
            }
        }

        private void RefreshTarget()
        {
            if (Target.IsNull() && commonDetectHelper.IsNotNull() && commonDetectHelper.Target.IsNotNull())
            {
                Target = commonDetectHelper.Target;
            }
        }


        Vector3 targetDir;
        float rotateCoeff = 2f;
        float coeff = 0f;
        private void CalcRotate()
        {            
            if (Vector3.Angle(targetDir, dir) > 2f)
            {
                dir = (targetDir * rotateCoeff * Time.deltaTime + dir).normalized;
                if(coeff < 1f)
                {
                    coeff += Time.deltaTime * 2;
                }
                if(rotateCoeff < speed)
                {
                    rotateCoeff += Time.deltaTime * speed * coeff * coeff * 0.3f;
                }
            }
            else
            {
                // 小于2度直接赋值
                dir = targetDir;
                coeff = 0f;
                rotateCoeff = 2f;
            }

            if (needRotate) CarrierGo.transform.LookAt(dir);
        }

        private bool IsTooCloseToTarget()
        {
            return Vector3.Distance(transform.position, Target.hitPosition.position) < 0.1f;
        }

        private void CalcDir()
        {
            targetDir = (Target.hitPosition.position - transform.position).normalized;
        }

        private void Move()
        {
            CarrierGo.transform.Translate(dir.normalized * Time.deltaTime * speed, Space.World);
        }

        private void HurtTimeTick()
        {
            foreach (var go in hurtTimeCount.Keys.ToList()) // .ToList() 防止修改时迭代出错
            {
                if (hurtTimeCount[go] > 0)
                {
                    hurtTimeCount[go] -= Time.deltaTime;
                }
            }
        }

        private void LifeTimeTick()
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime < 0)
            {
                Destroy();
            }
        }

        Dictionary<GameObject, float> hurtTimeCount;
        private void OnTriggerStay(Collider collision)
        {
            if (isDestroy) return;

            // 一个敌人上会有三个碰撞体，加上地面，也就是说一个子弹命中，这个方法会进4次
            // 如果同时命中多个，就会有3n + 1 次，这里以后看看需不需要优化
            DebugTool.Log(collision.gameObject.name.ToString());
            if (Owner.gameObject.IsNotMe(collision.gameObject) && collision.gameObject.CompareTag(TagConstant.Unit))
            {
                if (hurtTimeCount.ContainsKey(collision.gameObject))
                {
                    if (hurtTimeCount[collision.gameObject] < 0)
                    {
                        collision.gameObject.GetComponent<UnitEntity>().GetHurt(power);
                        hurtTimeCount[collision.gameObject] = hurtColdTime;
                    }
                }
                else
                {
                    collision.gameObject.GetComponent<UnitEntity>().GetHurt(power);
                    hurtTimeCount.Add(collision.gameObject, hurtColdTime);
                }

                if (destroyWhenHurt)
                {
                    Destroy();
                }
            }
        }

        public void Destroy()
        {
            isDestroy = true;
            hurtTimeCount.Clear();
            quickTimer.DestroyTimers();
            if (commonDetectHelper.IsNotNull())
            {
                commonDetectHelper.Destroy();
                commonDetectHelper = null;
            }
            PoolManager.GetCarrierPool().UnSpawn(this.gameObject, name);
        }
    }
}
