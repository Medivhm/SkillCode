namespace Magic
{
    public class ShootStraight1 : MagicStateBase
    {
        // 前摇时间
        float lateTime = 0.5f;
        public ShootStraight1(MagicFsm fsm, float size, float speed, float power, string baseBulletName, string fxName, float delLateTime) : base(fsm)
        {

        }
    }
}
