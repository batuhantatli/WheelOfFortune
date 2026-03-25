using System;
using TimerSystem;
using UnityEngine;
using UnityEngine.Events;


public class DataSaveManager : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private float saveInterval = 0.02f;

    private Timer _saveTimer;

    #endregion

    #region UNITY METHODS

    private void Start()
    {
        UnityEvent onTimerCompleteEvent = new UnityEvent();
        onTimerCompleteEvent.AddListener(DataManager.SaveAllCachedDatas);
        _saveTimer = new Timer(saveInterval, true, true, onTimerCompleteEvent, null);
    }

    private void Update()
    {
        _saveTimer?.Update();
    }

    #endregion
}