using Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Manager
{
    public class CarrierManager : Singleton<CarrierManager>
    {
        public void Init()
        {

        }

        public static CarrierInfo GetCarrierInfoByID(int carrierID)
        {
            CarrierInfo info;
            DataManager.Instance.CarrierInfos.TryGetValue(carrierID, out info);
            return info;
        }
    }
}
