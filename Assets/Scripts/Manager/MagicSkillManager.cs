using Info;
using Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Manager
{
    public class MagicSkillManager : Singleton<MagicSkillManager>
    {
        Dictionary<int, MagicSkill> MagicSkills;

        public void Init()
        {
            MagicSkills = new Dictionary<int, MagicSkill>()
            {
                { 1, new BeBig()   },
                { 2, new BePower() },
                { 3, new BeFast()  },
                { 4, new BeTwo()   },
                { 5, new BeThree() },
                { 6, new BeTrace() },
                { 7, new BeLock()  },
            };
        }

        public static MagicSkill GetMagicSkillByID(int msgicSkillID)
        {
            MagicSkill magicSkill;
            Instance.MagicSkills.TryGetValue(msgicSkillID, out magicSkill);
            return magicSkill;
        }
    }
}
