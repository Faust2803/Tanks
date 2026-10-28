using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "ColorsConfig", menuName = "Configs/Game/SO Colors Config", order = 0)]
    public class ColorsConfig : ScriptableObject
    {
        [SerializeField] private  List<ColorConfigItem> _colorList = null;
        public List<ColorConfigItem> ColorList => _colorList;
        public int GetColorsCount => _colorList.Count;
        public Color GetColor(int id)
        {
            for (var i = 0; i < _colorList.Count; i++)
            {
                if (_colorList[i].Id == id)
                {
                    return _colorList[i].Color;
                }
            }
            Debug.LogWarning("<color=#FF6600>Colors Config: Color not found!!!</color>");
            return Color.black;
        }
    }
    
    [Serializable]
    public class ColorConfigItem
    {
        public int Id;
        public string Name;
        public Color Color;
        public int Prise;
        public bool ShowInShop = false;
    }
    
}