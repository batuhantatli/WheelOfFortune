using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WhellOfFortune.Scripts.ZoneSystem
{
    public class ZoneCount : MonoBehaviour
    {
        public TMP_Text countText;
        public Image backgroundImage;

        public Sprite currentWhiteImage;
        public Sprite superImage;

        private RectTransform _baseContainer;
        private RectTransform _centerContainer;
        private RectTransform _rectTransform;

        private ZoneState _currentState;

        public bool isPassed;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(RectTransform baseContainer, RectTransform centerContainer, ZoneState initialState)
        {
            _baseContainer = baseContainer;
            _centerContainer = centerContainer;

            SetState(initialState, true);
            SetMaskable(true);
        }

        public void SetPosition(float itemSize, float centerX, float space, float index)
        {
            _rectTransform.anchorMin = new Vector2(0, 0.5f);
            _rectTransform.anchorMax = new Vector2(0, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            _rectTransform.sizeDelta = new Vector2(itemSize, itemSize);

            float xPos = centerX + index * (itemSize + space);
            _rectTransform.anchoredPosition = new Vector2(xPos, 0);
        }

        public void SetText(int index)
        {
            countText.text = index.ToString();
        }

        public void SetState(ZoneState state, bool instantBackground)
        {
            _currentState = state;

            ApplyTextVisual(state);
            if (state == ZoneState.Center)
            {
                SetMaskable(false);
                SetParent(ZoneState.Center);
            }
            if (instantBackground)
            {
                ApplyBackgroundVisual(state);
            }
        }
        
        public int GetNumber()
        {
            return int.TryParse(countText.text, out int n) ? n : 0;
        }

        public RectTransform GetRectTransform()
        {
            return _rectTransform;
        }

        public void SetPositionX(float x)
        {
            _rectTransform.anchoredPosition = new Vector2(x, _rectTransform.anchoredPosition.y);
        }

        private void ApplyTextVisual(ZoneState state)
        {
            switch (state)
            {
                case ZoneState.Center:
                    countText.color = Color.black;
                    countText.alpha = 1f;
                    break;

                case ZoneState.Passed:
                    countText.color = Color.white;
                    countText.alpha = 0.1f;
                    break;

                case ZoneState.Upcoming:
                    countText.color = Color.white;
                    countText.alpha = 1f;
                    break;
            }
        }

        // ✅ BACKGROUND AYRI
        private void ApplyBackgroundVisual(ZoneState state)
        {
            switch (state)
            {
                case ZoneState.Center:
                    SetBackgroundImage(currentWhiteImage, true);
                    break;

                case ZoneState.Passed:
                    SetBackgroundImage(currentWhiteImage, false);
                    break;

                case ZoneState.Upcoming:
                    SetBackgroundImage(currentWhiteImage, false);
                    break;
            }
        }

        private void SetMaskable(bool maskable)
        {
            countText.maskable = maskable;
        }

        private void SetParent(ZoneState state)
        {
            if (state == ZoneState.Center)
                transform.SetParent(_centerContainer);
            else
                transform.SetParent(_baseContainer);
        }

        private void SetBackgroundImage(Sprite sprite, bool isEnable)
        {
            backgroundImage.gameObject.SetActive(isEnable);

            if (!isEnable) return;

            backgroundImage.sprite = sprite;
        }

        public void MoveNext(float moveAmount, float moveSpeed)
        {
            Ease x = Ease.Linear;
            _rectTransform
                .DOAnchorPosX(-moveAmount, moveSpeed).SetEase(x)
                .SetRelative()
                .OnComplete(() =>
                {
                    if (isPassed)
                    {
                        return;
                    }
                    
                    if (_currentState != ZoneState.Passed) return;
                    
                    isPassed = true;
                    ApplyBackgroundVisual(ZoneState.Passed);
                    SetParent(ZoneState.Passed);
                    SetMaskable(true);
                });
        }
    }
}