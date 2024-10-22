using System;
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

    public void RemoveItemToHotBar(Item item, int index)
    {
        HotBar.Instance.SetItem(item, index);
        item.IndexMarker = index;
        RemoveItem(item);
    }

    public void RemoveItemToBody(Item item, BodyPos pos)
    {
        Body.Instance.SetItem(item, pos);
        item.IndexMarker = (int)pos;
        RemoveItem(item);
    }

    public void AddItem(ItemType itemType, int itemId, bool doChange = true, bool putAsSingle = false)
    {
        Item item = Ctrl.CreateItem(itemType, itemId);
        AddItemEx(item, doChange, putAsSingle);
    }

    public void AddItem(Item item, bool doChange = true, bool putAsSingle = false)
    {
        AddItemEx(item, doChange, putAsSingle);
    }

    private void AddItemEx(Item item, bool doChange, bool putAsSingle)
    {
        item.From = From.Bag;

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
            if (!putAsSingle)
            {
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


public class HotBar : Singleton<HotBar>
{
    public int gridsNum = 5;
    public Item[] Items;

    // 物品数量变化事件
    // + 数量增加 - 数量减少 a 增加物品 d 删除物品
    public UnityAction<Item, char, int> ItemChange;

    public HotBar()
    {
        Items = new Item[gridsNum];
    }

    public void SetItem(Item item, int index)
    {
        RemoveItemToBag(index);

        Items[index] = item;
        item.From = From.HotBar;
        SendChangeMessage(item, 'a', index);
    }

    public void RemoveItemToBag(int index)
    {
        if (Items[index].IsNull()) return;

        Bag.Instance.AddItem(Items[index], true, true);
        Items[index].From = From.Bag;
        Items[index].IndexMarker = 0;
        Items[index] = null;
        SendChangeMessage(null, 'd', index);
    }

    public void ChangeWithOtherGrid(int index1, int index2)
    {
        var temp1 = Items[index1];
        var temp2 = Items[index2];

        if (temp1.IsNotNull())
        {
            temp1.IndexMarker = index2;
            Items[index2] = temp1;
            SendChangeMessage(temp1, 'a', index2);
        }
        else
        {
            Items[index2] = null;
            SendChangeMessage(null, 'd', index2);
        }
        if (temp2.IsNotNull())
        {
            temp2.IndexMarker = index1;
            Items[index1] = temp2;
            SendChangeMessage(temp2, 'a', index1);
        }
        else
        {
            Items[index1] = null;
            SendChangeMessage(null, 'd', index1);
        }
    }

    public bool UseItem(Item item)
    {
        if (!From.HotBar.Equals(item.From)) return false;

        return UseItem(item.IndexMarker);
    }

    public bool UseItem(int index)
    {
        if (Items[index].IsNull()) return false;
        if (Items[index].Num < 1) return false;

        if (Items[index] is IUseable)
        {
            if(!(Items[index] as IUseable).Use())
            {
                // 不满足使用条件
                return false;
            }
        }
        else
        {
            // 是不可用Item
            return false;
        }

        if (Items[index].Num == 0)
        {
            Items[index] = null;
            SendChangeMessage(null, 'd', index);
        }
        else
        {
            SendChangeMessage(null, '-', index);
        }

        return true;
    }

    private void SendChangeMessage(Item item, char ch, int index)
    {
        if (ItemChange != null)
        {
            ItemChange(item, ch, index);
        }
    }
}

public enum BodyPos
{
    Head,
    LeftHeader,
    LeftArm,
    LeftHands,
    Leg,
    Body,
    RightHeader,
    RightArm,
    RightHands,
    Foot,
}
public class Body : Singleton<Body>
{
    public Item[] Items;

    // 物品数量变化事件
    // + 数量增加 - 数量减少 a 增加物品 d 删除物品
    public UnityAction<Item, char, BodyPos> ItemChange;

    public Body()
    {
        Items = new Item[Enum.GetValues(typeof(BodyPos)).Length];
    }

    int GetIndex(BodyPos pos)
    {
        return Convert.ToInt32(pos);
    }

    public void SetItem(Item item, BodyPos pos)
    {
        RemoveItemToBag(pos);

        Items[GetIndex(pos)] = item;
        item.From = From.Body;

        SendChangeMessage(item, 'a', pos);
    }

    public void RemoveItemToBag(BodyPos pos)
    {
        if (Items[GetIndex(pos)].IsNull()) return;

        Bag.Instance.AddItem(Items[GetIndex(pos)], true, true);
        Items[GetIndex(pos)].From = From.Bag;
        Items[GetIndex(pos)].IndexMarker = 0;
        Items[GetIndex(pos)] = null;

        SendChangeMessage(null, 'd', pos);
    }

    private void SendChangeMessage(Item item, char ch, BodyPos pos)
    {
        if (ItemChange != null)
        {
            ItemChange(item, ch, pos);
        }
    }
}