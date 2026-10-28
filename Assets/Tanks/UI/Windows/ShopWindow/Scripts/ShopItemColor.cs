using System;
using Managers;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using UnityEngine;

namespace UI.Windows
{
    public class ShopItemColor : ShopItem
    {
        private ColorConfigItem _configItem;
        public static Action<Color> AddColors ;
        
        public void CreateObjects(ColorConfigItem configItem,  PlayerManager playerManager, UiManager uiManager)
        {
            CreateObjects(playerManager, uiManager);
            _title.text = configItem.Name;
            _coastText.text = "Buy for "+configItem.Prise;
            _rimage.color = configItem.Color;
            _coast = configItem.Prise;
            _configItem = configItem;

            if (_playerManager.Data.Colors.IndexOf(_configItem.Color) != -1)
            { 
                _button.gameObject.SetActive(false);
            }
        }

        protected override void ByeItem()
        {
            _playerManager.AddColor(_configItem.Color);
            _button.gameObject.SetActive(false);
            _uiManager.ForceOpenWindow(WindowType.MessageWindow, new MessageWindowData
            {
                Title = "Purchase successful",
                Description = $"Congratulations, you have successfully purchased {_configItem.Name} color!.",
                HasCancelButton = false,
                HasOKButton = true
            });
            
            AddColors?.Invoke(_configItem.Color);
        }
    }
}