public class AttackAttr
{
    public float damage;                // 伤害
    public float attackSpeed;           // 攻击速度（/秒）
    public float critiOdds;             // 暴击几率（0，1）
    public float critiCoeff;            // 暴击伤害倍率（正常你得大于1）

    public AttackAttr() { }

    public AttackAttr(AttackAttr other)
    {
        this.damage = other.damage;
        this.attackSpeed = other.attackSpeed;
        this.critiOdds = other.critiOdds;
        this.critiCoeff = other.critiCoeff;
    }
}
