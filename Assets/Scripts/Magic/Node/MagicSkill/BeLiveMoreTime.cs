using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic
{
    public class BeLiveMoreTime : MagicSkill
    {
        public override void DoChange(Magic magic)
        {
            magic.liveTimeMult += 1;
        }
    }
}
