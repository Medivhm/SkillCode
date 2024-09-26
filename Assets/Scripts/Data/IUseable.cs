using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Useable
{
    public abstract string Icon
    {
        get;
    }

    public abstract bool Use();
}
