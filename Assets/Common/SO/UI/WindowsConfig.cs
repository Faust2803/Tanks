using System;
using System.Collections.Generic;
using UI.Panels;
using UI.Windows;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "WindowsConfig", menuName = "Configs/UI/SO Windows Config", order = 1)]
    public class WindowsConfig : ScriptableObject
    {
        [SerializeField] private  List<WindowsList> _windowList = null;

        public GameObject GetWindow(WindowType type)
        {
            for (var i = 0; i < _windowList.Count; i++)
            {
                if (_windowList[i].Id == type)
                {
                    return _windowList[i].Prefab;
                }
            }
            Debug.LogWarning("<color=#FF6600>WindowsConfig: Window not found!!!</color>");
            return null;
        }
    }
    
    
    [Serializable]
    public class WindowsList
    {
        public WindowType Id;
        public GameObject Prefab;
    }
}