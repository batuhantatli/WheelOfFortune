using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


    public static class DataManager
    {
        private const string Root = "Data";
        private static bool _isSaveRequested;
        private static Dictionary<string, string> _cachedData = new();

        #region HELPER

        private static string GetRootPath(string folderName = "")
        {
            string path = string.IsNullOrEmpty(folderName)
                ? Application.persistentDataPath + "/" + Root
                : Application.persistentDataPath + "/" + Root + "/" + folderName;
            return path;
        }

        private static string GetFilePath(string fileName, string folderName = "")
        {
            string path = string.IsNullOrEmpty(folderName)
                ? Application.persistentDataPath + "/" + Root + "/" + fileName + ".json"
                : Application.persistentDataPath + "/" + Root + "/" + folderName + "/" + fileName + ".json";
            return path;
        }

        #endregion

        #region PUBLIC METHODS

        public static string ReadDataWithPath(string fileName, string folderName)
        {
            if (!Directory.Exists(GetRootPath())) Directory.CreateDirectory(GetRootPath());
            if (!Directory.Exists(GetRootPath(folderName))) Directory.CreateDirectory(GetRootPath(folderName));

            string path = GetFilePath(fileName, folderName);

            if (!File.Exists(path)) return null;

            string jsonStringData = File.ReadAllText(path);
            return jsonStringData;
        }

        public static bool SaveDataWithPath(string fileName, string folderName, string stringData)
        {
            if (!Directory.Exists(GetRootPath())) Directory.CreateDirectory(GetRootPath());
            if (!Directory.Exists(GetRootPath(folderName))) Directory.CreateDirectory(GetRootPath(folderName));

            string path = GetFilePath(fileName, folderName);
            try
            {
                _cachedData[path] = stringData;
                _isSaveRequested = true;
                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"CANNOT WRITE DATA TO FILE \n {e.Message}");
            }
        }

        public static void SaveAllCachedDatas()
        {
            if (_isSaveRequested)
            {
                foreach (var pair in _cachedData)
                {
                    string path = pair.Key;
                    string stringData = pair.Value;
                    File.WriteAllText(path, stringData);
                }

                _cachedData.Clear();
                _isSaveRequested = false;
            }
        }

        #endregion

        #region EDITOR TOOLS

#if UNITY_EDITOR
        [MenuItem("SaveData/Delete All Saved Data")]
        public static void Delete()
        {
            try
            {
                if (!Directory.Exists(GetRootPath())) return;

                Directory.Delete(GetRootPath(), true);
                Directory.Delete(Application.persistentDataPath, true);
                PlayerPrefs.DeleteAll();

                Debug.Log("ALL FILE DELETE SUCCESS");
            }
            catch (Exception e)
            {
                throw new Exception($"FILES DELETE : {e.Message}");
            }
        }
#endif

        #endregion
    }
