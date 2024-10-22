using Info;
using Manager;
using QEntity;

public class Prop : Item, IUseable
{
    public Prop(int propId, ItemType itemType, From from, int num, UnitEntity owner = null) 
        : this(
              ItemManager.GetPropInfo(propId), 
              itemType, 
              from, 
              num, 
              owner)
    {
    }

    public Prop(PropInfo propInfo, ItemType itemType, From from, int num, UnitEntity owner = null) 
        : base(
            itemType, 
            from, 
            num, 
            new ItemInfo()
            {
                ID = propInfo.ID,
                type = propInfo.type,
                name = propInfo.name,
                color = propInfo.color,
                overlap = propInfo.overlap,
                prefab = propInfo.prefab,
                icon = propInfo.icon,
                desc = propInfo.desc,
            }, 
            owner)
    {
        
    }

    public bool Use()
    {
        this.Num--;
        GUI.ActionInfoLog(string.Format("使用了[{0}]", Name));
        return true;
    }
}
