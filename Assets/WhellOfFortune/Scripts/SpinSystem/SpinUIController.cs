using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WhellOfFortune.Scripts.InventorySystem;
using WhellOfFortune.Scripts.ManagerSystem;
using WhellOfFortune.Scripts.RewardSystem;
using WhellOfFortune.Scripts.RewardSystem.Rewards;
using WhellOfFortune.Scripts.WheelRewards;
using WhellOfFortune.Scripts.ZoneSystem;
using Random = UnityEngine.Random;

namespace WhellOfFortune.Scripts.SpinSystem
{
    public class SpinUIController : BaseUIController
    {
        
        [Header("References")] 
        [SerializeField] private Transform rotateAnimationContainer;
        [SerializeField] private Transform spawnAnimationContainer; 
        [SerializeField] private Image spinImage;
        [SerializeField] private Image spinIndicatorImage;
        [SerializeField] private Button spinButton;

        
        [Header("Reward Spawn")] [SerializeField]
        private RectTransform rewardContainer;

        [SerializeField] private RewardItemUI rewardPrefab;
        [SerializeField] private float radius = 200f;

        [Header("Spin Settings")] [SerializeField]
        private float spinDuration = 3f;

        [SerializeField] private int minSpinCount = 3;
        [SerializeField] private int maxSpinCount = 6;
        [SerializeField] private AnimationCurve spinCurve;

        [Header("Collected Panel")] 
        [SerializeField] private Button exitButton;
        
        private List<BaseSpinRewardData> _currentRewards = new List<BaseSpinRewardData>();
        private List<RewardItemUI>  _rewards = new List<RewardItemUI>();

        private bool _isSpinning;
        private int SliceCount => _currentRewards.Count;
        private float AnglePerSlice => 360f / SliceCount;
        
        private ZoneController _zoneController;
        private CollectedRewardController _collectedRewardController;
        private DeathUIController _deathUIController;
        private InventoryUIController _inventoryUIController;
        public CollectedRewardController CollectedRewardController => _collectedRewardController;
        

        #region INIT

        public override void InitializeUIController(GeneralUIManager uiManager)
        {
            _deathUIController = GameManager.Instance.GetManager<GeneralUIManager>().GetUIController<DeathUIController>();
            _inventoryUIController = uiManager.GetUIController<InventoryUIController>();
            
            _zoneController = GetComponent<ZoneController>();
            _zoneController.Initialize();

            _collectedRewardController = GetComponent<CollectedRewardController>();
            _collectedRewardController.Initialize(_inventoryUIController);
            
            exitButton.onClick.AddListener((() =>
            {
                _collectedRewardController.ExitZone();
                ResetRewards();
                ClosePanel();
            }));
            spinButton.onClick.AddListener(Spin);
            
            base.InitializeUIController(uiManager);
            
        }

        #endregion

        #region OVERRIDE

        protected override void OpenPanel()
        {
            DOVirtual.DelayedCall(1, SetSpin);
            base.OpenPanel();
        }

        public override void ClosePanel()
        {
            ClearSpin();
            _inventoryUIController.TryOpenPanel(null);
            base.ClosePanel();
        }

        #endregion

        #region PRIVATE METHODS

        private void SetSpin()
        {
            _zoneController.SpawnZonesCount();
            _zoneController.OnNextZoneLoading?.Invoke();
        }

        private void Spin()
        {
            if (_isSpinning || SliceCount == 0)
                return;

            StartCoroutine(SpinCoroutine());
        }

        private IEnumerator SpinCoroutine()
        {
            _isSpinning = true;

            float time = 0f;

            float startAngle = NormalizeAngle(rotateAnimationContainer.eulerAngles.z);

            int targetIndex = Random.Range(0, SliceCount);

            float extraSpins = Random.Range(minSpinCount, maxSpinCount) * 360f;

            // Slice center
            float sliceCenter = targetIndex * AnglePerSlice;

            // Pointer yukarı = 90°
            float targetAngle = NormalizeAngle(90f - sliceCenter); // ← normalize et
            float angleDiff = targetAngle - NormalizeAngle(startAngle);
            if (angleDiff < 0) angleDiff += 360f; // her zaman ileri dönsün

            float finalAngle = startAngle + extraSpins + angleDiff;

            while (time < spinDuration)
            {
                time += Time.deltaTime;

                float t = Mathf.Clamp01(time / spinDuration);
                float curveT = spinCurve.Evaluate(t);

                float currentAngle = Mathf.LerpUnclamped(startAngle, finalAngle, curveT);

                rotateAnimationContainer.rotation = Quaternion.Euler(0, 0, currentAngle);

                yield return null;
            }

            rotateAnimationContainer.rotation = Quaternion.Euler(0, 0, finalAngle);

            int finalIndex = GetIndexFromAngle(finalAngle);

            Debug.Log("Target: " + targetIndex + " | Result: " + finalIndex);

            StartCoroutine(OnSpinComplete(finalIndex));

            _isSpinning = false;
        }

        private int GetIndexFromAngle(float angle)
        {
            float normalized = NormalizeAngle(angle);

            // Pointer yukarı olduğu için +90 kaydır
            float adjusted = NormalizeAngle(90f - normalized);

            int index = Mathf.FloorToInt(adjusted / AnglePerSlice) % SliceCount;

            return index;
        }

        private IEnumerator OnSpinComplete(int index)
        {
            if (index < 0 || index >= _currentRewards.Count)
            {
                yield return null;
            }
            

            BaseSpinRewardData rewardData = _currentRewards[index];
            if (rewardData is BombSpinRewardData)
            {
                _deathUIController.TryOpenPanel(null);
                yield break;
            }
            
            rewardData.Reward();
            Debug.Log(rewardData.RewardImage);
            yield return new WaitForSeconds(0.5f);
            SpawnAnimation(spawnAnimationContainer,false ,()=>
            {
                ResetRewards();
                _zoneController.NextZone();
                _zoneController.OnNextZoneLoading?.Invoke();
                SpawnAnimation(spawnAnimationContainer,true);
            });
        }

        private void SpawnRewards()
        {
            if (rewardContainer == null || rewardPrefab == null || _currentRewards.Count == 0)
                return;

            for (int i = rewardContainer.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(rewardContainer.GetChild(i).gameObject);
            }

            for (int i = 0; i < _currentRewards.Count; i++)
            {
                // float angle = i * AnglePerSlice + (AnglePerSlice / 2f);
                float angle = i * AnglePerSlice;

                float rad = angle * Mathf.Deg2Rad;

                float x = Mathf.Cos(rad) * radius;
                float y = Mathf.Sin(rad) * radius;

                RewardItemUI item = Instantiate(rewardPrefab, rewardContainer);

                RectTransform rect = item.GetComponent<RectTransform>();
                BaseSpinRewardData  rewardData = _currentRewards[i];
                rect.anchoredPosition = new Vector2(x, y);
                rect.localRotation = Quaternion.Euler(0, 0,AnglePerSlice*i -90);
                item.Initialize(rewardData,rewardData.RewardImage,rewardData.rewardValue);
                _rewards.Add(item);
            }
        }

        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0) angle += 360f;
            return angle;
        }

        private void ClearSpin()
        {
            ResetRewards();
            _zoneController.ResetZone();
            _collectedRewardController.ResetCollectedRewards();
        }

        private void ResetRewards()
        {
            foreach (var VARIABLE in _rewards)
            {
                Destroy(VARIABLE.gameObject);
            }
            _rewards.Clear();
            _currentRewards.Clear();
        }

        private void SpawnAnimation(Transform wheelParent, bool isSpawning,Action OnComplete = null)
        {
            // DOTween sequence oluştur
            Sequence seq = DOTween.Sequence();


            if (isSpawning)
            {
                // Başlangıç durumunu ayarla
                wheelParent.localScale = Vector3.zero;
                wheelParent.localRotation = Quaternion.Euler(0, 0, 0);

                // Scale ve Rotate animasyonu aynı anda ekle
                seq.Append(wheelParent.DOScale(1f, 1f).SetEase(Ease.OutBack))
                    .Join(wheelParent.DOLocalRotate(new Vector3(0, 0, 720f), 1f, RotateMode.FastBeyond360)
                        .SetEase(Ease.OutCubic)).OnComplete((() =>
                    {
                        OnComplete?.Invoke();
                    }));
            }
            else
            {

                // Kaybolma animasyonu
                seq.Append(wheelParent.DOScale(0f, 1f).SetEase(Ease.InBack))
                    .Join(wheelParent.DOLocalRotate(new Vector3(0, 0, -720f), 1f, RotateMode.FastBeyond360)
                        .SetEase(Ease.InCubic)).OnComplete((() =>
                    {
                        OnComplete?.Invoke();
                    }));
            }
        }

        #endregion


        #region PUBLIC METHODS

        public void SetSpinType(Sprite spinSprite , Sprite spinIndicator)
        {
            spinImage.sprite = spinSprite;
            spinIndicatorImage.sprite = spinIndicator;
        }

        public void SetRewards(List<BaseSpinRewardData> rewards)
        {
            _currentRewards.AddRange(rewards);
            SpawnRewards();
        }

        #endregion

    }
}