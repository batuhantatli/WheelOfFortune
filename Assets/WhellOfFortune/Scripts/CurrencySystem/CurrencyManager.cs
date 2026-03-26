using System;
using System.Collections.Generic;
using UnityEngine;
using WhellOfFortune.Scripts.ManagerSystem;

namespace WhellOfFortune.Scripts.CurrencySystem
{
    public class CurrencyManager : BaseManager
    {
        #region FIELDS

        public static event Action<CurrencyType, float, CurrencyAnimationInfo> OnCurrencyValueChangeWithAnimationInfo;

        public static event Action<CurrencyType, float> OnCurrencyValueChange;
        public CurrencyCanvasSetup CurrencyCanvasSetupRef => currencyCanvasSetupRef;

        [field: SerializeField] public List<CurrencyData> currencyData;
        [SerializeField] private CurrencyCanvasSetup currencyCanvasSetupRef;

        #endregion

        #region PUBLIC METHODS

        public void SpendCurrency(CurrencyType cType, float spendVal)
        {
            foreach (var data in currencyData)
            {
                if (data.currencyType == cType)
                {
                    if (data.UserData.currentCurrencyValue >= spendVal)
                    {
                        data.UserData.currentCurrencyValue -= spendVal;
                        var updatedValue = data.UserData.currentCurrencyValue;
                        OnCurrencyValueChangeWithAnimationInfo?.Invoke(cType, updatedValue, default);
                        OnCurrencyValueChange?.Invoke(cType, updatedValue);
                        return;
                    }
                }
            }

            Debug.LogWarning($"Can not find currency data info for Spend Currency = {cType}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cType : Currency Type"></param>
        /// <param name="resourceVal : Currency add value"></param>
        public void AddCurrency(CurrencyType cType, float resourceVal)
        {
            foreach (var data in currencyData)
            {
                if (data.currencyType == cType)
                {
                    data.UserData.currentCurrencyValue += resourceVal;
                    data.SaveUserData();
                    var updatedValue = data.UserData.currentCurrencyValue;
                    OnCurrencyValueChangeWithAnimationInfo?.Invoke(cType, updatedValue, default);
                    OnCurrencyValueChange?.Invoke(cType, updatedValue);
                    return;
                }
            }

            Debug.LogWarning($"Can not find currency data info for Add Currency = {cType}");
        }

        public void AddCurrencyWithAnimation(CurrencyType cType, float resourceVal, Transform startPoint,
            bool isWorldPoint, float startScaleMultiplier = 1)
        {
            foreach (var data in currencyData)
            {
                if (data.currencyType == cType)
                {
                    data.UserData.currentCurrencyValue += resourceVal;
                    var updatedValue = data.UserData.currentCurrencyValue;
                    OnCurrencyValueChangeWithAnimationInfo?.Invoke(cType, updatedValue,
                        new CurrencyAnimationInfo()
                        {
                            isAnimationSet = true,
                            pointInfo = startPoint,
                            isWorldPos = isWorldPoint,
                            startScaleMultiplier = startScaleMultiplier
                        });
                    OnCurrencyValueChange?.Invoke(cType, updatedValue);
                    return;
                }
            }

            Debug.LogWarning($"Can not find currency data info for Add Currency = {cType}");
        }

        public void TriggerOnlyCurrencyAnimation(CurrencyType cType,Transform startPoint,
            bool isWorldPoint, float startScaleMultiplier = 1)
        {
            foreach (var data in currencyData)
            {
                if (data.currencyType == cType)
                {
                    var updatedValue = data.UserData.currentCurrencyValue;
                    OnCurrencyValueChangeWithAnimationInfo?.Invoke(cType, updatedValue,
                        new CurrencyAnimationInfo()
                        {
                            isAnimationSet = true,
                            pointInfo = startPoint,
                            isWorldPos = isWorldPoint,
                            startScaleMultiplier = startScaleMultiplier
                        });
                    return;
                }
            }

            Debug.LogWarning($"Can not find currency data info for Add Currency = {cType}");
        }

        public float GetCurrencyValue(CurrencyType cType)
        {
            foreach (var data in currencyData)
            {
                if (data.currencyType == cType)
                {
                    return data.UserData.currentCurrencyValue;
                }
            }

            Debug.LogWarning($"Can not find currency data info for = {cType}");
            return 0;
        }

        public bool IsAffordable(CurrencyType cType ,float itemValue)
        {
            return GetCurrencyValue(cType) >= itemValue;
        }

        public Sprite GetCurrencyIcon(CurrencyType cType)
        {
            foreach (var data in currencyData)
            {
                if (data.currencyType == cType)
                {
                    return data.currencyIcon;
                }
            }

            Debug.LogWarning($"Can not find currency icon for = {cType}");
            return null;
        }

        #endregion

        #region MANAGER SETTINGS

        public override void LoadManager(Action onLoadEnd)
        {
            foreach (var data in currencyData)
            {
                data.Initialize();
            }
        
            onLoadEnd?.Invoke();
        }
        
        public override void InitManager()
        {
            if (currencyCanvasSetupRef != null)
            {
                currencyCanvasSetupRef.InitCurrencyUIs(this);
            }
        }
        
        #endregion


    }

    public struct CurrencyAnimationInfo
    {
        public Transform pointInfo;
        public bool isAnimationSet;
        public bool isWorldPos;
        public float startScaleMultiplier;
    }
}