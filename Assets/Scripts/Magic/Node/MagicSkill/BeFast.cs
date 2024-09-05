using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic
{
    public class BeFast : MagicSkill
    {
        public override void DoChange(Magic magic)
        {
            magic.speedMult += 1;
        }
    }
}
