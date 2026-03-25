using System;
using SaveSystem;
using UnityEditor;
using UnityEngine;

    public abstract class BaseScriptableData : ScriptableObject
    {
        [Header("Main Config")]
        public string objectName;
        public WorldName worldName;

        public abstract string GetUserDataModelID();
    }

    public abstract class BaseScriptableData<TUserData> : BaseScriptableData where TUserData : BaseDataModel<TUserData>
    {
        #region FIELDS

        [Header("Default User Data")]
        [SerializeField] protected TUserData defaultUserData;

        private bool _isInitialized;
        private string _folderName;
        private string FolderName
        {
            get
            {
                if (string.IsNullOrEmpty(_folderName))
                {
                    _folderName = $"{worldName}/{GetType().Name}/";
                }

                return _folderName;
            }
        }

        private TUserData _activeUserData;
        public TUserData UserData
        {
            get
            {
                if (_activeUserData == null || string.IsNullOrEmpty(_activeUserData.id))
                {
                    LoadUserData();
                }

                return _activeUserData;
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void Initialize()
        {
            if (_isInitialized) return;
            LoadUserData();
            _isInitialized = true;
        }

        public sealed override string GetUserDataModelID()
        {
            return defaultUserData.id;
        }

        public void SaveUserData()
        {
            DataManager.SaveDataWithPath(defaultUserData.id, FolderName, _activeUserData.ToJsonString());
        }

        #endregion

        #region PRIVATE METHODS

        protected virtual void LoadUserData()
        {
            if (_isInitialized) return;
            var userDataFromStorage = DataManager.ReadDataWithPath(defaultUserData.id, FolderName)
                .ToJsonObject<TUserData>();
            if (userDataFromStorage == null || string.IsNullOrEmpty(userDataFromStorage.id))
            {
                userDataFromStorage = defaultUserData.Clone();
                userDataFromStorage.id = defaultUserData.id;
            }
            _activeUserData = userDataFromStorage;
            SaveUserData();
        }

        #endregion

        #region EDITOR

        protected virtual void OnValidateChild()
        {
        }

#if UNITY_EDITOR

        private void OnEnable() => EditorApplication.playModeStateChanged += OnplayModeStateChanged;
        private void OnDisable() => EditorApplication.playModeStateChanged -= OnplayModeStateChanged;

        private void OnplayModeStateChanged(PlayModeStateChange newState)
        {
            switch (newState)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    ResetActiveDatas();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void ResetActiveDatas()
        {
            if (_activeUserData != null)
            {
                _activeUserData.id = "";
            }

            _activeUserData = null;
            _folderName = string.Empty;
            _isInitialized = false;
        }

        protected void OnValidate()
        {
            OnValidateChild();
            string userDataID = defaultUserData.id;
            if (string.IsNullOrEmpty(userDataID))
            {
                defaultUserData.id = Guid.NewGuid().ToString();
            }
        }

#endif

        #endregion
    }
