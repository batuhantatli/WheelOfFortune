using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using GAReport;
using UnityEngine;
using WhellOfFortune.Scripts.ManagerSystem;

    public class GameManager : MonoBehaviour
    {
        #region FIELDS

        public static GameManager Instance { get; private set; }
        
        [SerializeField] private List<BaseManager> baseManagers = new();

        public List<BaseManager> Managers => baseManagers;
        
        private Dictionary<string, BaseManager> _cacheManagers;
        private int _loadCount;

        #endregion

        #region INIT

        private void Awake()
        {
            Instance = this;
            _cacheManagers = new();

            _loadCount = baseManagers.Count;
            foreach (var manager in baseManagers)
            {
                manager.SetGameManager(this);
                manager.LoadManager(() =>
                {
                    _loadCount--;
                });
            }

            StartCoroutine(InitManagers());
            
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            // GAReportManager.SendGameStartEvent();
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// General method for getting manager.
        /// </summary>
        /// <typeparam name="T: Manager type"></typeparam>
        /// <returns></returns>
        public T GetManager<T>() where T : BaseManager
        {
            var classKey = typeof(T).FullName;
            if (_cacheManagers.TryGetValue(classKey, out BaseManager cacheManager))
            {
                return cacheManager as T;
            }

            foreach (var manager in baseManagers)
            {
                var resultM = manager as T;
                if (resultM != null)
                {
                    _cacheManagers.TryAdd(classKey, resultM);
                    return resultM;
                }
            }

            Debug.LogError($"Can not find target manager");
            return default;
        }

        #endregion

        #region PRIVATE METHODS

        
        private IEnumerator InitManagers()
        {
            while (_loadCount > 0)
            {
                yield return null;
            }

            foreach (var manager in baseManagers)
            {
                manager.InitManager();
            }
        }

        #endregion
    }
