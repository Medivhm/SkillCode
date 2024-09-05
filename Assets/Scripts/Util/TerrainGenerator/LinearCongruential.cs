using UnityEngine;

// 线性同余生成器
public class LinearCongruential
{
    // S[i+1] = (a * S[i] + b) mod m

    private int current;
    private int a;  // 乘积系数
    private int b;  // 相加系数
    private int m;  // 取模，使生成数在 [0, m)
    private int seed;

    public LinearCongruential(int seed = 3333, int a = 1102, int b = 7213, int m = 10000)
    {
        this.a = a;
        this.b = b;
        this.m = m;
        this.seed = seed;
        this.current = seed;
    }

    public float NextFloat()
    {
        current = (a * current + b) % m;
        if (IsPositive())
        {
            return (float)current / m;
        }
        else
        {
            return -(float)current / m;
        }
    }

    public bool IsPositive()
    {
        return Random.value > 0.5f;
    }
}