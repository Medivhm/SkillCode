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
    public partial class BrickEntity : MonoBehaviour
    {
        public void Init()
        {

        }

        public void Destroy()
        {
            PoolManager.GetBrickPool().UnSpawn(this.gameObject, name);
        }
    }
}
