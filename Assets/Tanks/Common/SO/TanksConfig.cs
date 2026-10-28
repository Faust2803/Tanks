using System;
using System.Collections.Generic;
using Tanks.Common.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "TanksConfig", menuName = "Configs/Game/SO Tank Config", order = 0)]
    public class TanksConfig : ScriptableObject
    {
        
        [SerializeField] private  List<TanksConfigItem> _tanksList = null;
        public List<TanksConfigItem> TanksList => _tanksList;
        public int TanksCount => _tanksList.Count;
        public TanksConfigItem GetTank(int id)
        {
            for (var i = 0; i < _tanksList.Count; i++)
            {
                if (i == id)
                {
                    return _tanksList[i];
                }
            }
            Debug.LogWarning("<color=#FF6600>Tanks Config: Tank not found!!!</color>");
            return null;
        }
    }
    
    [Serializable]
    public class TanksConfigItem
    {
        public int Id;
        public string Name;
        public GameObject Prefab;
        public int Prise;
        public bool ShowInShop = false;
    }
    
}