using System;
using Managers;
using SO.Scripts;
using Tanks.Common.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Windows
{
    public abstract class ShopItem : MonoBehaviour
    {
        [SerializeField] protected RawImage  _rimage;
        [SerializeField] protected Button  _button;
        [SerializeField] protected TextMeshProUGUI  _title;
        [SerializeField] protected TextMeshProUGUI _coastText;

        protected  PlayerManager _playerManager;
        protected UiManager _uiManager;
        protected int _coast;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnByeClicked);
        }
        
        public  void CreateObjects(PlayerManager playerManager, UiManager uiManager)
        {
            _playerManager = playerManager;
            _uiManager = uiManager;
        }

        protected virtual void OnByeClicked()
        {
            if (_playerManager.ChangeCurrency(-_coast))
            {
                ByeItem();
            }
            else
            {
                _uiManager.ForceOpenWindow(WindowType.NotEnafMoney, new NotEnafMoneyWindowData
                {
                    Amount = _coast - _playerManager.Data.Currency
                });
            }
        }

        protected abstract void ByeItem();

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}