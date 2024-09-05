using QEntity;
using Info;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    class ItemManager : Singleton<ItemManager>
    {
        public void Init()
        {

        }

        static ItemInfo itemInfo;
        public static Item CreateItem(int itemID)
        {
            if (GetItemInfo(itemID).IsNotNull())
            {
                return new Item(itemInfo, ItemFrom.Other, 1);
            }
            return null;
        }

        public static ItemInfo GetItemInfo(int itemID)
        {
            if (!DataManager.Instance.ItemInfos.TryGetValue(itemID, out itemInfo))
            {
                DebugTool.Error("ItemID 错误");
                return null;
            }
            return itemInfo;
        }
    }
}
