using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WhellOfFortune.Scripts.ManagerSystem;
using WhellOfFortune.Scripts.SpinSystem;

namespace WhellOfFortune.Scripts.ZoneSystem
{
    public class ZoneController : MonoBehaviour
    {
        public ZoneData bronzeZoneData;
        public ZoneData silverZoneData;
        public ZoneData goldZoneData;

        public RectTransform baseContainer;
        public RectTransform centerContainer;
        public ZoneCount zoneCount;

        public float space;
        public int count = 13;
        public float moveSpeed;

        private List<ZoneCount> items = new List<ZoneCount>();
        private int currentIndex = 1;
        private float itemSize;

        private ZoneCount _currentCenterZone;
        private ZoneBase _currentZone;
        private SpinUIController _spinUIController;
        
        public Action OnNextZoneLoading;
        
        #region INIT

        public void Initialize()
        {
            _spinUIController = GetComponent<SpinUIController>();
            OnNextZoneLoading += SetZone;
        }
        #endregion

        #region PRIVATE METHODS

        private void SetZone()
        {
            _currentZone = ZoneFactory.GetZone(currentIndex, bronzeZoneData, silverZoneData, goldZoneData);
            _currentZone.SetSpinType(_spinUIController);
            SetCenterZone(items[currentIndex-1]);
        }

        public void SpawnZonesCount()
        {
            float containerWidth = baseContainer.rect.width;

            float totalSpacing = space * (count + 1);
            itemSize = (containerWidth - totalSpacing) / count;

            float centerX = containerWidth / 2f;

            items.Clear();

            for (int i = 0; i < count; i++)
            {
                ZoneCount newItem = Instantiate(zoneCount, baseContainer);
                newItem.Initialize(baseContainer, centerContainer, ZoneState.Upcoming);
                newItem.SetText(i+1);
                // var _rectTransform = newItem.GetComponent<RectTransform>();
                // _rectTransform.anchorMin = new Vector2(0, 0.5f);
                // _rectTransform.anchorMax = new Vector2(0, 0.5f);
                // _rectTransform.pivot = new Vector2(0.5f, 0.5f);
                //
                // _rectTransform.sizeDelta = new Vector2(itemSize, itemSize);
                //
                // float xPos = centerX + i * (itemSize + space);
                // _rectTransform.anchoredPosition = new Vector2(xPos, 0);
                newItem.SetPosition(itemSize, centerX, space, i);
                items.Add(newItem);
            }

            currentIndex = 1;
            SetCenterZone(items[currentIndex]);
        }

        private void SetCenterZone(ZoneCount newCenterZone)
        {
            if (_currentCenterZone == null)
            {
                _currentCenterZone = newCenterZone;
                _currentCenterZone.SetState(ZoneState.Center, true);
                return;
            }
            
            _currentCenterZone.SetState(ZoneState.Passed, false);

            newCenterZone.SetState(ZoneState.Center, true);

            _currentCenterZone = newCenterZone;
        }

        public void NextZone()
        {
            if (items.Count == 0) return;
            if (currentIndex >= items.Count - 1) return;

            currentIndex++;

            float moveAmount = itemSize + space;

            foreach (var item in items)
            {
                item.MoveNext(moveAmount, moveSpeed);
            }

            SetCenterZone(items[currentIndex-1]);
        }

        public void ResetZone()
        {
            foreach (var item in items)
            {
                Destroy(item.gameObject);
            }
            
            items.Clear();

            currentIndex = 0;
        }

        #endregion
    }
}