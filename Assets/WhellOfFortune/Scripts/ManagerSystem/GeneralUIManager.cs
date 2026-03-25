using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WhellOfFortune.Scripts.ManagerSystem;

namespace WhellOfFortune.Scripts
{
    public class GeneralUIManager : BaseManager
    {
        #region FIELDS

        [SerializeField] private List<BaseUIController> uiControllers = new();

        private bool _isOpenUIDisabled;
        public bool IsInitCompleted { get; private set; }

        #endregion

        #region PUBLIC METHODS
        
        public T GetUIController<T>() where T : BaseUIController
        {
            foreach (var controller in uiControllers)
            {
                if(controller is T)
                    return controller as T;
            }
            Debug.LogError($"No controller found for {typeof(T).Name}");
            return null;
        }
        
        // Disable RayCast of panels. 
        public void DisableRayCastPanels(params BaseUIController[] exceptPanels)
        {
            foreach (var uiPanel in uiControllers)
            {
                if(exceptPanels.Contains(uiPanel))
                    continue;
                uiPanel.SetStateOfRayCast(false);
            }
        }

        // Enable RayCast of panels. 
        public void EnableRayCastPanels()
        {
            foreach (var uiPanel in uiControllers)
            {
                uiPanel.SetStateOfRayCast(true);
            }
        }
        
        public void SetInvisibleCanvases(params BaseUIController[] exceptPanels)
        {
            foreach (BaseUIController uiPanel in uiControllers)
            {
                if(exceptPanels.Contains(uiPanel))
                    continue;
                uiPanel.SetCanvasAlpha(0);
            }
        }

        public void SetVisibleAllCanvases()
        {
            foreach (BaseUIController uiPanel in uiControllers)
            {
                uiPanel.SetCanvasAlpha(1);
            }
        }

        // Check all ui panels for are there any front panel is already open.
        public bool AreThereAnyFrontPanelActive()
        {
            foreach (var uiController in uiControllers)
            {
                if (uiController.isFrontCanvas && uiController.IsOpen)
                    return true;
            }
            return false;
        }
        
        public bool IsOpenPanelDisabled() => _isOpenUIDisabled;

        public void SetStateOfOpenPanelUI(bool isOpenUIDisabled) => _isOpenUIDisabled = isOpenUIDisabled; 

        #endregion
        
        #region INIT/LOAD MANAGER

        public override void LoadManager(Action onLoadEnd)
        {
            // Set references of ui managers.
            foreach (var uiController in uiControllers)
            {
                uiController.InitializeUIController(this);
            }
            
            onLoadEnd?.Invoke();
        }

        public override void InitManager()
        {
            foreach (var uiController in uiControllers)
            {
                var isActiveAtStart = uiController.isActiveAtStart;
                uiController.mainPanel.SetActive(isActiveAtStart);
            }

            IsInitCompleted = true;
        }
        
        #endregion
    }
}
