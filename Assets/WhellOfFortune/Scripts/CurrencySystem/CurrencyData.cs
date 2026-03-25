using UnityEngine;

namespace WhellOfFortune.Scripts.CurrencySystem
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "Game/Currency/CurrencyData", order = 1)]
    public class CurrencyData : BaseScriptableData<CurrencySaveDataModel>
    {
        [Header("Currency Elements")]
        public CurrencyType currencyType;
        public float startCurrencyValue;
        public Sprite currencyIcon;
    }

    [System.Serializable]
    public class CurrencySaveDataModel : BaseDataModel<CurrencySaveDataModel>
    {
        public bool isSet;
        public float currentCurrencyValue;
    }

}