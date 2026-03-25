namespace WhellOfFortune.Scripts.ZoneSystem
{
    public static class ZoneFactory
    {
        public static ZoneBase GetZone(int index, ZoneData normalConfig, ZoneData safeConfig, ZoneData superConfig)
        {
            if (index % 30 == 0 && index >= 30)
                return new SuperZone(index, superConfig);

            if (index % 5 == 0 && index >=5)
                return new SafeZone(index, safeConfig);

            return new BronzeZone(index, normalConfig);
        }
    }
}