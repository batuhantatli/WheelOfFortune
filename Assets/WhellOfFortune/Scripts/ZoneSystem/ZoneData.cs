using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.SpinSystem;

[CreateAssetMenu(fileName = "Zone Type" , menuName = "Game/Zone Type Data",order = 1)]
public class ZoneData : ScriptableObject
{
    public string zoneName;
    public Sprite spin;
    public Sprite spinIndicator;
    public List<BaseSpinRewardData> rewards;
}
