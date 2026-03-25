using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.ZoneSystem
{
    public class SuperZone : ZoneBase
    {
        public SuperZone(int index, ZoneData config) : base(index, config) { }
        public override void SetSpinType(SpinUIController wheel)
        {
            base.SetSpinType(wheel);
            // wheel.PlaySuperAnimation();
        }
    }
}