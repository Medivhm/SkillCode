using Manager;
using UnityEngine;

namespace QEntity
{

    public class FxEntity : MonoBehaviour
    {
        public Vector3 dir;
        public float speed;
        public bool isInit;

        // 是固定不动的
        bool isStable = true;

        public void Init(string name, Vector3? dir, float speed)
        {
            this.name = name;
            if (dir != null)
            {
                isStable = false;
                this.dir = (Vector3)dir;
            }
        
            this.speed = speed;
        }

        public void Update()
        {
            Move();
        }

        void Move()
        {
            if (isStable) return;
            if (this.gameObject == null) return;

            float dt = Time.deltaTime;
            //this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            this.gameObject.transform.position += dir.normalized * dt * speed;
        }

        public void Destroy()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
