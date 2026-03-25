using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WhellOfFortune.Scripts.CurrencySystem
{
    public class CurrencyUIControl : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private CurrencyType currencyType;
        [Space]
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private RectTransform currencyUIIconRef;

        [Header("Money Animation Settings")] 
        [SerializeField] private int currencyCount = 5;
        [SerializeField] private float delayBetweenCurrency = .15f;
        [SerializeField] private float eachCurrencyMovementTime = .7f;

        private CurrencyManager _cacheManager;
        private Vector3 _moneyIconBaseScale;
        private bool _isCurrencyBounceAnimationOn;
        private float _lastValue;
        private Camera _mainCamera;
        private Tween _outOfCurrencyTween;
        private bool _outOfCurrencyAnimationOn;
        
        public CurrencyType CurrencyType => currencyType;
        
        #endregion

        #region INIT
        
        public void InitCurrencyUI(CurrencyManager currencyManager)
        {
            _cacheManager = currencyManager;
            _mainCamera = Camera.main;  // TODO : needs refactor
            _moneyIconBaseScale = currencyUIIconRef.localScale;
            CurrencyManager.OnCurrencyValueChangeWithAnimationInfo += HandleCurrencyValueUpdate;
            UpdateCurrencyText(_cacheManager.GetCurrencyValue(currencyType),false);
        }
        
        private void OnDestroy()
        {
            CurrencyManager.OnCurrencyValueChangeWithAnimationInfo -= HandleCurrencyValueUpdate;
        }

        #endregion

        #region PRIVATE METHODS

        private void HandleCurrencyValueUpdate(CurrencyType cType, float val,
            CurrencyAnimationInfo animationInfo)
        {
            if (cType != currencyType)
                return;
            bool isAnimOn = animationInfo.isAnimationSet;
            float bounceDelay = isAnimOn ? eachCurrencyMovementTime : 0;
            UpdateCurrencyText(val,isAnimOn,eachCurrencyMovementTime);
            ApplyMoneyBounceAnimation(bounceDelay);
            if(isAnimOn)
                StartMoneyAnimation(animationInfo.pointInfo,animationInfo.isWorldPos,
                    currencyCount,delayBetweenCurrency,null,animationInfo.startScaleMultiplier);
        }
        
        private void StartMoneyAnimation(Transform startPos,bool isWorldPos,int moneyCount,float eachMovementDelay,
            Action onComplete,float startScaleMultiplier = .2f)
        {
            if(_mainCamera == null)
                _mainCamera = Camera.main;
            
            Vector3 targetPoint = isWorldPos ? _mainCamera.WorldToScreenPoint(startPos.position) : startPos.position;
            StartCoroutine(MoneyAnimationSequence(targetPoint, moneyCount, eachMovementDelay, startScaleMultiplier,
                eachCurrencyMovementTime, onComplete));
        }
        
        private IEnumerator MoneyAnimationSequence(Vector3 targetPos,int moneyCount,float eachDelay,float scaleMultiplier,
            float eachDuration,Action onAnimationComplete)
        {
            int currentMoneyCount = moneyCount;
            WaitForSeconds delay = new WaitForSeconds(eachDelay);
            while (currentMoneyCount > 0)
            {
                var newCurrency = Instantiate(currencyUIIconRef, currencyUIIconRef.parent);
                newCurrency.localScale = _moneyIconBaseScale * scaleMultiplier;
                newCurrency.position = targetPos;
                newCurrency.DOScale(_moneyIconBaseScale, eachDuration * .9f);
                newCurrency.DOMove(currencyUIIconRef.transform.position, eachDuration).OnComplete(() =>
                {
                    Destroy(newCurrency.gameObject);
                });
                
                currentMoneyCount--;

                yield return delay;
            }
            onAnimationComplete?.Invoke();
        }
        
        private void ApplyMoneyBounceAnimation(float startDelay)
        {
            if (_isCurrencyBounceAnimationOn) return;

            _isCurrencyBounceAnimationOn = true;

            Vector3 targetScale = _moneyIconBaseScale * 1.2f;
            currencyUIIconRef.DOScale(targetScale, .15f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                _isCurrencyBounceAnimationOn = false;
            }).SetDelay(startDelay);
        }
        
        private void UpdateCurrencyText(float val,bool withAnim,float startDelay = 0)
        {
            if (!withAnim)
            {
                _lastValue = val;
                currencyText.text = Mathf.Floor(val).ToString();
                return;
            }

            float refStartValue = _lastValue;
            float targetValue = val;
            _lastValue = val;
            DOTween.To(x =>
            {
                float result = refStartValue + ((targetValue-refStartValue) * x);
                currencyText.text = Mathf.Round(result).ToString();
            }, 0, 1, 1).OnComplete(() =>
            {
                currencyText.text = Mathf.Floor(targetValue).ToString();
            }).SetDelay(startDelay);
        }

        #endregion
        
        #region PUBLIC METHODS
        public void ActivateOutOfMoneyAnimation()
        {
            if (_outOfCurrencyAnimationOn)
                return;

            _outOfCurrencyAnimationOn = true;
            var targetScale = _moneyIconBaseScale * 1.15f;
            _outOfCurrencyTween = currencyUIIconRef.DOScale(targetScale, 0.4f).SetLoops(-1, LoopType.Yoyo);
        }

        public void DeActivateOutOfMoneyAnimation()
        {
            if (!_outOfCurrencyAnimationOn)
                return;

            _outOfCurrencyAnimationOn = false;
            _outOfCurrencyTween?.Kill();
            currencyUIIconRef.localScale = _moneyIconBaseScale;
        }
        
        #endregion
    }
}
