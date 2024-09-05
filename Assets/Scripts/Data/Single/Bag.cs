using System.Collections.Generic;
using UnityEngine.Events;

public class Bag : Singleton<Bag>
{
    public Dictionary<int, List<Item>> items = new Dictionary<int, List<Item>>();

    // 物品数量变化事件
    // + 数量增加 - 数量减少 a 增加物品 d 删除物品
    public UnityAction<Item, char> ItemChange;

    public void RemoveItem(Item item)
    {
        int itemID = item.ItemID;
        items[itemID].Remove(item);
        if (items[itemID].Count == 0)
        {
            items.Remove(itemID);
        }
        SendChangeMessage(item, 'd');
    }

    public void AddItem(int itemId, bool doChange = true)
    {
        Item item = Ctrl.CreateItem(itemId);
        AddItemEx(item, doChange);
    }

    public void AddItem(Item item, bool doChange = true)
    {
        AddItemEx(item, doChange);
    }

    private void AddItemEx(Item item, bool doChange)
    {
        item.From = ItemFrom.Bag;

        List<Item> list;
        if (!items.TryGetValue(item.ItemID, out list))
        {
            list = new List<Item>();
            list.Add(item);
            items.Add(item.ItemID, list);
            if (doChange)
            {
                SendChangeMessage(item, 'a');
            }
        }
        else
        {
            int num = item.Num;
            foreach (var itemInBag in list)
            {
                if (itemInBag.Num < itemInBag.Overlap)
                {
                    int canPutNum = itemInBag.Overlap - itemInBag.Num;
                    if (num <= canPutNum)
                    {
                        itemInBag.Num += num;
                        num = 0;
                    }
                    else
                    {
                        itemInBag.Num = itemInBag.Overlap;
                        num -= canPutNum;
                    }
                    if (doChange)
                    {
                        SendChangeMessage(itemInBag, '+');
                    }
                    if (num == 0)
                    {
                        // 如果已经放完了
                        break;
                    }
                }
            }
            if (num > 0)
            {
                item.Num = num;
                list.Add(item);
                if (doChange)
                {
                    SendChangeMessage(item, 'a');
                }
            }
        }
    }

    public void UseItem(Item item, int num)
    {
        if (item.Num < num)
        {
            DebugTool.ErrorFormat("[Bag.cs] Item数量小于使用数量");
            return;
        }
        else if (item.Num == num)
        {
            RemoveItem(item);
        }
        else
        {
            item.Num -= num;
            SendChangeMessage(item, '-');
        }
    }

    private void SendChangeMessage(Item item, char ch)
    {
        if (ItemChange != null)
        {
            ItemChange(item, ch);
        }
    }
}
