using QEntity;
using Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;


class BuffManager : Singleton<BuffManager>, ITickable
{
    class Buff
    {
        public bool IsRunning
        {
            get { return isRunning; }
        }

        bool isRunning;
        UnitEntity makeUnit;
        UnitEntity targetUnit;
        float wholeTime;
        float reapeatTime;
        Action<UnitEntity, UnitEntity> overCB;
        Action<UnitEntity, UnitEntity> repeatCB;

        float total;
        float repeat;
        int times = -1;


        public Buff(UnitEntity makeUnit, UnitEntity targetUnit, Action<UnitEntity, UnitEntity> startCB, float wholeTime, Action<UnitEntity, UnitEntity> overCB, float reapeatTime , Action<UnitEntity, UnitEntity> repeatCB, bool doAtFirst = false, int times = -1)
        {
            total = 0;
            isRunning = true;

            this.makeUnit = makeUnit;
            this.targetUnit = targetUnit;
            this.reapeatTime = reapeatTime;
            this.wholeTime = wholeTime;
            this.repeatCB = repeatCB;
            this.overCB = overCB;
            this.times = times;

            if(startCB != null)
            {
                startCB(makeUnit, targetUnit);
                startCB = null;
            }
            if (repeatCB != null)
            {
                if (doAtFirst)
                {
                    repeat = reapeatTime + 1;
                }
                else
                {
                    repeat = 0;
                }
            }
        }

        public void Tick(float dt)
        {
            TickWhole(dt);
            TickRepeat(dt);
        }

        public void TickWhole(float dt)
        {
            if (times != -1) { return; }

            total += dt;
            if (total >= wholeTime)
            {
                Stop();
            }
        }

        public void TickRepeat(float dt)
        {
            if (repeatCB == null) return;

            repeat += dt;
            if (repeat > reapeatTime)
            {
                repeat = 0;
                if(times != -1)
                {
                    times--;
                    if (times == 0) Stop();
                }
                repeatCB(makeUnit, targetUnit);
            }
        }

        public bool IsOver()
        {
            return total >= wholeTime;
        }

        public void Stop()
        {
            isRunning = false;
            if(overCB != null)
            {
                overCB(makeUnit, targetUnit);
                overCB = null;
            }
        }
    };

    static Dictionary<int, Buff> buffs = new Dictionary<int, Buff>();
    List<int> deleteBuffs = new List<int>();

    static private int buffId;
    static private int BuffId
    {
        get
        {
            int temp = buffId;
            int rangeTimes = 0;
            while (buffs.ContainsKey(temp) && rangeTimes < BuffConstant.BuffMax)
            {
                temp = (temp + 1) % BuffConstant.BuffMax;
                rangeTimes++;
            }
            if (rangeTimes == BuffConstant.BuffMax)
            {
                DebugTool.Error("buff超数量了");
                throw new Exception("buff超数量了");
            }
            else
            {
                buffId = temp;
                return temp;
            }
        }
        set
        {
            buffId = value;
        }
    }

    public void Init()
    {
        BuffId = 0;
        TickHelper.Instance.Add(Instance);
    }

    public void Update()
    {
        float deltaTime = Time.deltaTime;
        BuffTick(deltaTime);
        BuffDestroy();
    }

    private void BuffTick(float deltaTime)
    {
        List<int> buffKeys = buffs.Keys.ToList();
        foreach (int key in buffKeys)
        {
            Buff buff = null;
            buffs.TryGetValue(key, out buff);
            if (buff != null)
            {
                if (buff.IsRunning)
                {
                    buff.Tick(deltaTime);
                }
                else
                {
                    deleteBuffs.Add(key);
                }
            }
        }
    }

    private void BuffDestroy()
    {
        foreach (int key in deleteBuffs)
        {
            if (buffs.TryGetValue(key, out _))
            {
                buffs.Remove(key);
            }
        }
        deleteBuffs.Clear();
    }

    // 全参数Add
    public static int Add(UnitEntity makeUnit, UnitEntity targetUnit, Action<UnitEntity, UnitEntity> startCB, float wholeTime, Action<UnitEntity, UnitEntity> overCB, float reapeatTime, Action<UnitEntity, UnitEntity> repeatCB, bool doAtFirst = false, int times = -1)
    {
        int buffId = BuffId;
        buffs.Add(buffId, new Buff(makeUnit, targetUnit, startCB, wholeTime, overCB, reapeatTime, repeatCB, doAtFirst, times));
        return buffId;
    }

    // 持续时间Add
    public static int Add(UnitEntity makeUnit, UnitEntity targetUnit, float reapeatTime, Action<UnitEntity, UnitEntity> repeatCB, bool doAtFirst = false, int times = 1)
    {
        int buffId = BuffId;
        buffs.Add(buffId, new Buff(makeUnit, targetUnit, null, -1, null, reapeatTime, repeatCB, doAtFirst, times));
        return buffId;
    }

    /// <summary>
    /// 触发次数Add
    /// </summary>
    /// <param name="makeUnit">发起单位</param>
    /// <param name="targetUnit">目标单位</param>
    /// <param name="reapeatTime">间隔时间</param>
    /// <param name="times">次数</param>
    /// <param name="repeatCB">触发回调</param>
    /// <param name="doAtFirst">是否立刻触发一次</param>
    /// <returns>buffId</returns>
    public static int Add(UnitEntity makeUnit, UnitEntity targetUnit, float reapeatTime, int times, Action<UnitEntity, UnitEntity> repeatCB, bool doAtFirst = false)
    {
        int buffId = BuffId;
        buffs.Add(buffId, new Buff(makeUnit, targetUnit, null, -1, null, reapeatTime, repeatCB, doAtFirst, times));
        return buffId;
    }

    // 仅首尾触发Add
    public static int Add(UnitEntity makeUnit, UnitEntity targetUnit, Action<UnitEntity, UnitEntity> startCB, float wholeTime, Action<UnitEntity, UnitEntity> overCB)
    {
        int buffId = BuffId;
        buffs.Add(buffId, new Buff(makeUnit, targetUnit, startCB, wholeTime, overCB, -1, null, false, -1));
        return buffId;
    }

    public static void Remove(int key)
    {
        Buff buff = null;
        buffs.TryGetValue(key, out buff);
        if (buff != null)
        {
            buff.Stop();
        }
    }

    public void LateUpdate()
    {
        
    }
}