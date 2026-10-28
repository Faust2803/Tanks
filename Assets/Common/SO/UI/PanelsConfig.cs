using System;
using System.Collections.Generic;
using UI.Panels;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "PanelsConfig", menuName = "Configs/UI/SO Panels Config", order = 0)]
    public class PanelsConfig : ScriptableObject
    {
        
        [SerializeField] private  List<PanelsList> _panelsList = null;

        public GameObject GetPanel(PanelType type)
        {
            for (var i = 0; i < _panelsList.Count; i++)
            {
                if (_panelsList[i].Id == type)
                {
                    return _panelsList[i].Prefab;
                }
            }
            Debug.LogWarning("<color=#FF6600>Panels Config: Panel not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class PanelsList
    {
        public PanelType Id;
        public GameObject Prefab;
    }
}