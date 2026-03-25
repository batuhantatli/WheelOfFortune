using UnityEngine;
using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.ZoneSystem
{
    public abstract class ZoneBase
    {
        public int zoneIndex;         // hangi zone olduğunu tutar
        public ZoneData config;     // editor’dan gelen data

        public ZoneBase(int index, ZoneData config)
        {
            this.zoneIndex = index;
            this.config = config;
        }

        // Wheel’a zone özelliklerini uygular
        public virtual void SetSpinType(SpinUIController wheel)
        {
            Debug.Log(config.rewards.Count);
            wheel.SetSpinType(config.spin,config.spinIndicator);
            wheel.SetRewards(config.rewards);
        }
    }
}