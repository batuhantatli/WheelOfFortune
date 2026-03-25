using System;
using UnityEngine;

namespace WhellOfFortune.Scripts.ManagerSystem
{
    public abstract class BaseManager : MonoBehaviour
    {
        #region FIELDS
        
        protected GameManager _gameManager; 

        #endregion

        #region LOAD/INIT METHODS


        public void SetGameManager(GameManager gameManager) => _gameManager = gameManager; 
        
        /// <summary>
        /// Loading to manager for initial setup of internal system.(Do not try to reach other managers in this method, they not might be load)
        /// </summary>
        /// <param name="manager"></param>
        public abstract void LoadManager(Action onLoadEnd);

        /// <summary>
        /// After loading all managers, init manager for game start, can access to other managers and game references if needed
        /// </summary>
        public abstract void InitManager();
        
        #endregion
        
    }
}
