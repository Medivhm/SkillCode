using System.Collections.Generic;

public class TickHelper : MonoSingleton<TickHelper>
{
    List<ITickable> ticks = new List<ITickable>();

    private void Update()
    {
        foreach(var tick in ticks)
        {
            tick.Update();
        }     
    }

    private void LateUpdate()
    {
        foreach (var tick in ticks)
        {
            tick.LateUpdate();
        }
    }

    public void AddRange(List<ITickable> ticks)
    {
        ticks.AddRange(ticks);
    }

    public void Add(ITickable tick)
    {
        ticks.Add(tick);
    }

    public void Remove(ITickable tick)
    {
        ticks.Remove(tick);
    }

    public void Clear()
    {
        ticks.Clear();
    }
}
