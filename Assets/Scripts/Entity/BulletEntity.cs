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
        private float? lifeTime;
        // 发射方向
        private Vector3 dir;
        // 自动瞄准目标，有则发射方向始终指向该目标，否则指定方向
        private Transform Target;
        private GameObject CarrierGo;
        private UnitEntity Owner;
        private QuickTimer quickTimer;

        public bool isDestroy = false;
        public bool isInit = false;

        public float power;
        public float speedMult;
        public float scale;


        public void Init(CarrierInfo info, Vector3 pos, float? lifeTime, Vector3? dir, Transform Target, GameObject CarrierGo, UnitEntity Owner, float power = 1f, float speedMult = 1f, float scale = 1f)
        {
            Init(info.speed, pos, lifeTime, dir, Target, CarrierGo, Owner, power, speedMult, scale);
        }

        public void Init(float speed, Vector3 pos, float? lifeTime, Vector3? dir, Transform Target, GameObject CarrierGo, UnitEntity Owner, float power, float speedMult, float scale)
        {
            transform.SetParent(Main.SceneRoot);
            this.power = power;
            this.speedMult = speedMult;
            transform.localScale = new Vector3(scale, scale, scale);
            quickTimer = new QuickTimer(); 
            quickTimer.Init();
            this.speed = speed;
            this.lifeTime = lifeTime;
            this.dir = dir ?? Vector3.forward;
            this.Target = Target;
            this.CarrierGo = CarrierGo;
            this.Owner = Owner;
            this.transform.position = pos;
            this.transform.localEulerAngles = new Vector3(0, Util.GetDegY((Vector3)dir), 0);

            if (lifeTime != null)
            {
                quickTimer.AddTimer((float)lifeTime, () =>
                {
                    Destroy();
                });
            }
            isInit = true;
            isDestroy = false;
        }

        public void Update()
        {
            if (!isInit) return;

            if (!isDestroy)
            {
                if (Target != null)
                {
                    CarrierGo.transform.LookAt(Target.position);
                    dir = CarrierGo.transform.forward;
                }
                CarrierGo.transform.Translate(dir.normalized * Time.deltaTime * speed, Space.World);
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (isDestroy) return;

            // 一个敌人上会有三个碰撞体，加上地面，也就是说一个子弹命中，这个方法会进4次
            // 如果同时命中多个，就会有3n + 1 次，这里以后看看需不需要优化
            if (collision.gameObject.layer == Constant.Layer.Enemy)
            {
                collision.gameObject.GetComponent<UnitEntity>().GetHurt(power);
                Destroy();
            }
        }

        public void Destroy()
        {
            isDestroy = true;
            quickTimer.DestroyTimers();
            PoolManager.GetCarrierPool().UnSpawn(this.gameObject, name);
        }
    }
}
