using System;
using System.Collections.Generic;
using UnityEngine;


namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Game/SO Level Config", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private  List<LevelConfigItem> _levelConfigList = null;

        public int GetLevelCount => _levelConfigList.Count;
        
        public LevelConfigItem GetLevel(int id)
        {
            for (var i = 0; i < GetLevelCount; i++)
            {
                if (_levelConfigList[i].Id == id)
                {
                    return _levelConfigList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>[LevelConfig] Level not found!!!</color>");
            return _levelConfigList[0];
        }
    }
    
    [Serializable]
    public class LevelConfigItem
    {
        public int Id;
        public string Name;
        public GameObject LevelPrefab;
        public bool Active = true;
    }
    
}