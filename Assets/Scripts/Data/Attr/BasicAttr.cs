public class BasicAttr
{
    public float defence;              // 防御
    public float moveSpeed;            // 移动速度
    public float maxMoveSpeed;         // 最大移动速度
    public float minMoveSpeed;         // 最小移动速度

    public float blood;                // 血量
    public float power;                // 耐力
    public float stamina;              // 法力

    public float bloodMax;             // 血量最大
    public float powerMax;             // 耐力最大
    public float staminaMax;           // 法力最大


    public float bloodRecover;         // 血量恢复
    public float powerRecover;         // 耐力恢复
    public float staminaRecover;       // 法力恢复

    public BasicAttr() { }

    public BasicAttr(BasicAttr other)
    {
        this.defence = other.defence;
        this.moveSpeed = other.moveSpeed;
        this.maxMoveSpeed = other.maxMoveSpeed;
        this.minMoveSpeed = other.minMoveSpeed;
        this.blood = other.blood;
        this.power = other.power;
        this.stamina = other.stamina;
        this.bloodMax = other.bloodMax;
        this.powerMax = other.powerMax;
        this.staminaMax = other.staminaMax;
        this.bloodRecover = other.bloodRecover;
        this.powerRecover = other.powerRecover;
        this.staminaRecover = other.staminaRecover;
    }
}
