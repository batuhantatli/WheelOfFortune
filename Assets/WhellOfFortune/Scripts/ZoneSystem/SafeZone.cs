using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.ZoneSystem
{
    public class SafeZone : ZoneBase
    {
        public SafeZone(int index, ZoneData config) : base(index, config) { }


        public override void SetSpinType(SpinUIController wheel)
        {
            base.SetSpinType(wheel);
            // wheel.PlaySuperAnimation();
        }
    }
}