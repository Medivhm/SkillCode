using System;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Pool
{

    public class UIObject : ObjectBase
    {
        public override GameObject Spawn()
        {
            return base.Spawn();
        }

        public override void Unspawn(string name, GameObject target)
        {
            base.Unspawn(name, target);
            Target.MoveTo9999();
        }

        public override void Release()
        {
            if (!InUse)
            {
                GameObject.Destroy(Target);
            }
        }
    }
}
