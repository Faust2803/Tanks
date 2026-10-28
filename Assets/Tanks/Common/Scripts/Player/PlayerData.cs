using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tanks.Common.Scripts.Player
{
    [Serializable]
    public class PlayerData
    {
        public List<ColorData> TankColors;
        public List<int> AllTanksIds;
        public int CurrentTankId;
        public List<Color> Colors;
        public int Currency;
    }
    
    [Serializable]
    public class ColorData
    {
        public List<Color> Colors;
    }
}