using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WhellOfFortune.Scripts;

[RequireComponent(typeof(CanvasGroup),typeof(Canvas),typeof(GraphicRaycaster))]
    public abstract class BaseUIController : MonoBehaviour
    {
        #region FIELDS

        public Action OnPanelOpen;
        public Action OnPanelClose;
        
        [field: SerializeField] public bool isFrontCanvas { get; private set; }
        [field:SerializeField] public bool isActiveAtStart { get; private set; }
        [field: Header("Animations")]
        [field:SerializeField] public bool isCloseScaleAnimationDisabled { get; private set; }
        [field:SerializeField] public bool isSlidingOpenAnimation { get; private set; }
        [field:SerializeField] public float slideStartY { get; private set; }
        [field: Space]
        
        [field:SerializeField] public GameObject mainPanel { get; private set; }
        [SerializeField] protected Transform parentTransform;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected GraphicRaycaster graphicRayCaster;
        [SerializeField] protected AnimationCurve openEaseCurve;
        [SerializeField] protected AnimationCurve closeEaseCurve;
        
        protected GeneralUIManager GeneralUIManager;
        public bool IsOpen { get; protected set; }
        private Action _openPanelAction;

        #endregion
        
        #region PUBLIC METHODS

        public virtual void InitializeUIController(GeneralUIManager uiManager) => GeneralUIManager = uiManager;
        
        public void SetStateOfRayCast(bool state) => graphicRayCaster.enabled = state;

        public virtual void TryOpenPanel(Action onPanelOpen)
        {
            if (!GeneralUIManager.IsOpenPanelDisabled() && !GeneralUIManager.AreThereAnyFrontPanelActive())
            {
                OpenPanel();
                onPanelOpen?.Invoke();
                return;
            }
            
            // Start coroutine for the waits until open is enable.
            _openPanelAction = onPanelOpen;
            StartCoroutine(WaitingForOpenSequence());
        }

        public virtual void ClosePanel()
        {
            if(!isCloseScaleAnimationDisabled)
                parentTransform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(closeEaseCurve);
            canvasGroup.DOFade(0f, 0.3f).SetDelay(isSlidingOpenAnimation ? 0f : 0.2f).OnComplete(() =>
            {
                IsOpen = false;
                mainPanel.SetActive(false);
                OnPanelClose?.Invoke();
            });
        }

        public virtual void SetCanvasAlpha(float value)
        {
            canvasGroup.alpha = value;
        }
        
        #endregion

        #region PRIVATE METHODS

        protected virtual void OpenPanel()
        {
            mainPanel.SetActive(true);
            IsOpen = true;
            // Default open animation.
            OpenPanelAnimationHandle();
            OnPanelOpen?.Invoke();
        }

        private IEnumerator WaitingForOpenSequence()
        {
            yield return null;
            while (GeneralUIManager.IsOpenPanelDisabled() || GeneralUIManager.AreThereAnyFrontPanelActive())
            {
                yield return null;
            }
            
            // Else activate this panel
            OpenPanel();
            _openPanelAction?.Invoke();
        }

        protected virtual void OpenPanelAnimationHandle()
        {
            if (isSlidingOpenAnimation)
            {
                var rectTransform = mainPanel.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, slideStartY);
                rectTransform.DOAnchorPosY(0f, 0.5f).SetEase(openEaseCurve);
            }
            else
            {
                parentTransform.localScale = Vector3.one * 0.8f;
                parentTransform.DOScale(Vector3.one, 0.5f).SetEase(openEaseCurve);
            }
            canvasGroup.alpha = 0;
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, 0.5f);
        }

        #endregion
    }
