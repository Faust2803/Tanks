using System;
using Managers;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using UnityEngine;

namespace UI.Windows
{
    public class ShopItemTank : ShopItem
    {
        private TanksConfigItem _tanksConfigItem;
        public static Action<TanksConfigItem> AddTank ;
        public void CreateObjects(TanksConfigItem tanksConfigItem, Texture2D texture, PlayerManager playerManager, UiManager uiManager)
        {
            CreateObjects(playerManager, uiManager);
            _title.text = tanksConfigItem.Name;
            _coastText.text = "Buy for "+tanksConfigItem.Prise;
            _rimage.texture = texture;
            _coast = tanksConfigItem.Prise;
            _tanksConfigItem = tanksConfigItem;
            
            if (_playerManager.Data.AllTanksIds.IndexOf(_tanksConfigItem.Id) != -1)
            { 
                _button.gameObject.SetActive(false);
            }
        }
        
        protected override void ByeItem()
        {
            _playerManager.AddTank(_tanksConfigItem.Id);
            _button.gameObject.SetActive(false);
            _uiManager.ForceOpenWindow(WindowType.MessageWindow, new MessageWindowData
            {
                Title = "Purchase successful",
                Description = $"Congratulations, you have successfully purchased {_tanksConfigItem.Name} tank!.",
                HasCancelButton = false,
                HasOKButton = true
            });
            
            AddTank?.Invoke(_tanksConfigItem);
        }
    }
}