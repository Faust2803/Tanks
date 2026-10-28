using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SO.Scripts;

using UnityEngine;

namespace Tanks.Common.Scripts.Player
{
    public class PlayerManager
    {
        private const string FileName = "player_data.json";

        private PlayerData _data;
        private string SavePath => Path.Combine(Application.persistentDataPath, FileName);
        public PlayerData Data => _data;
        
        private readonly ConfigsManager _configsManager;
        private readonly ColorsConfig _colorConfig;
        
        public static Action<int> OnCurrencyChanged;
        
        public int CurrentMapId { get;  set; }
        public int CurrentPlayerCount { get;  set; }

        public PlayerManager(ConfigsManager configsManager, ColorsConfig colorConfig)
        {
            _configsManager = configsManager;
            _colorConfig = colorConfig;
            Load();
        }

        // =========================================================
        // LOAD
        // =========================================================

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                CreateDefaultData();
                Save();
                return;
            }

            try
            {
                var json = File.ReadAllText(SavePath);
                _data = JsonUtility.FromJson<PlayerData>(json);
                if (_data == null)
                {
                    CreateDefaultData();
                }
                ValidateData();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"<color=#FF0000>Failed to load player data: {e}</color>");
                CreateDefaultData();
            }

            /////
            Data.Currency = 1000;
            Save();
        }

        // =========================================================
        // SAVE
        // =========================================================

        public void Save()
        {
            try
            {
                var json = JsonUtility.ToJson(_data, true);
                File.WriteAllText(SavePath, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"<color=#FF0000>Failed to save player data: {e}</color>");
            }
        }

        // =========================================================
        // DEFAULT DATA
        // =========================================================

        private void CreateDefaultData()
        {
            var colors = new List<Color>();
            for (var i = 0; i < 5; i++)
            {
                colors.Add(_colorConfig.GetColor(i));
            }
            
            _data = new PlayerData
            {
                TankColors = new List<ColorData>(),
                AllTanksIds = new List<int>(),
                CurrentTankId = 0,
                Colors = colors,
                Currency = 0
            };
                
            for (var i = 0; i < 5; i++)
            {
                AddTank(_configsManager.TanksConfig.GetTank(i).Id);
            }
        }

        // =========================================================
        // VALIDATION
        // =========================================================

        private void ValidateData()
        {
            _data.TankColors ??= new();
            _data.AllTanksIds ??= new();
            _data.Colors ??= new();
            
            if (_data.AllTanksIds.Count == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    AddTank(_configsManager.TanksConfig.GetTank(i).Id);
                }
            }
            
            if (!_data.AllTanksIds.Contains(_data.CurrentTankId))
            {
                _data.CurrentTankId = _data.AllTanksIds[0];
            }

            if (_data.Colors.Count == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    _data.Colors.Add(_colorConfig.GetColor(i));
                }
            }
        }

        // =========================================================
        // TANK
        // =========================================================
        

        public int GetCurrentTank()
        {
            return _data.CurrentTankId;
        }

        public bool SetCurrentTank(int tankId)
        {
            if (!HasTank(tankId))
            {
                return false;
            }
            else
            {
                _data.CurrentTankId = tankId;
                Save();
                return true;
            }
            
        }

        public bool HasTank(int tankId)
        {
            return _data.AllTanksIds.Contains(tankId);
        }

        public void AddTank(int tankId)
        {
            if (_data.AllTanksIds.Contains(tankId)) return;
            _data.AllTanksIds.Add(tankId);
            
            SetColorTank( tankId,0, _configsManager.ColorsConfig.GetColor(0));
            SetColorTank( tankId,1, _configsManager.ColorsConfig.GetColor(0));
            SetColorTank( tankId,2, _configsManager.ColorsConfig.GetColor(1));
                
            Save();
        }

        // =========================================================
        // COLORS
        // =========================================================

        public void SetColorTank(int indexT,int index, Color color)
        {
            while (_data.TankColors.Count <= indexT)
            {
                var colorArr = new List<Color>();
                colorArr.Add(_configsManager.ColorsConfig.GetColor(0));
                colorArr.Add(_configsManager.ColorsConfig.GetColor(0));
                colorArr.Add(_configsManager.ColorsConfig.GetColor(1));
                
                var newColor = new ColorData();
                newColor.Colors = colorArr;
                _data.TankColors.Add(newColor);
            }

            _data.TankColors[indexT].Colors[index] = color;

            Save();
        }

        public Color GetColorTank(int indexT, int index)
        {
            if (index < 0 || index >= _data.TankColors.Count || indexT < 0 || indexT >= _data.TankColors.Count)
            {
                return Color.white;
            }
            return _data.TankColors[indexT].Colors[index];
        }

        public void AddColor(Color color)
        {
            if (_data.Colors.IndexOf(color) == -1)
            {
                _data.Colors.Add(color);
                Save();
            }
        }

        public bool ChangeCurrency(int value)
        {
            if (value < 0)
            {
                if (Data.Currency + value < 0)
                {
                    return false;
                }
                else
                {
                    Data.Currency += value;
                    Save();
                    OnCurrencyChanged?.Invoke(Data.Currency);
                    return true;
                }
            } 
            else
            {
                Data.Currency += value;
                Save();
                OnCurrencyChanged?.Invoke(Data.Currency);
                return true;
            }
        }
        
    }
}