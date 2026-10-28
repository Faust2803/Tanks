using System;
using Cysharp.Threading.Tasks;
using SO.Scripts;
using Tanks.Common.Scripts;
using Tanks.Common.Scripts.Player;
using UI.Windows;
using UnityEngine;
using Util;
using Utils;
using Zenject;

namespace UI.Panels
{
    public class LobbyMainPanelMediator :BasePanelMediator <LobbyMainPanelView, LobbyMainPanelData>
    {
        private PlayerManager _playerManager;
        private SceneLoader _sceneLoader;

        private Carusele _carusele = new Carusele();
        private CharacterRotator _rotator;

        private int _colorButtonCount = 0;
         
        [Inject]
        private void Construct(SceneLoader sceneLoader, PlayerManager playerManager)
        {
            _sceneLoader = sceneLoader; 
            _playerManager = playerManager;
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            
            _carusele.Items = Data.ItemsLoader.GetItems;
            _carusele.SetCurrentIndex( _playerManager.GetCurrentTank()) ;
            _carusele.Start();
            
            Target.StartButton.onClick.AddListener(OnStartButtonClicked);
            Target.ShopButton.onClick.AddListener(OnShopButtonClicked);
            Target.LeftButton.onClick.AddListener(ToNext);
            Target.RightButton.onClick.AddListener(ToPrevious);
            Target.RotateButtonListener.Pressed += RotateObject;
            Target.RotateButtonListener.Released += StopRotateObject;
            Target.BankButton.onClick.AddListener(OnBankButton);

            if (_colorButtonCount == 0)
            {
                for (var i = 0; i < _playerManager.Data.Colors.Count; i++)
                {
                    CreateColor(_playerManager.Data.Colors[i]);
                }
            }
            
            _rotator = new CharacterRotator(_carusele.Items[_playerManager.GetCurrentTank()], 200);
            Target.Rotator = _rotator;
            
            ShopItemColor.AddColors += CreateColor;
            ShopItemTank.AddTank += AddTank;
            
            OnCurrencvCange();
        }

        private async void AddTank(TanksConfigItem obj)
        {
            var tank = Data.ItemsLoader.AddTank(obj);
            await UniTask.Delay(10);
            _carusele.Items = Data.ItemsLoader.GetItems;
            _carusele.SetCurrentIndex( _playerManager.GetCurrentTank()) ;
            _carusele.Start();
        }

        private void CreateColor(Color color)
        {
            var button = Target.CreateColorButton(color);
            button.onClick.AddListener(() => OnColorButtonClicked(color));
            _colorButtonCount++;
        }

        private void OnColorButtonClicked(Color color)
        {
            var colorTypeValue = 0;
            for (var i = 0; i < Target.ColorLevelToggle.Count; i++)
            {
                if (Target.ColorLevelToggle[i].isOn)
                {
                    colorTypeValue = i;
                }
            }
            
            var tankId = _playerManager.GetCurrentTank();
            ColorChanger.Change(_carusele.Items[tankId].gameObject, color, colorTypeValue);
            _playerManager.SetColorTank(tankId, colorTypeValue, color);
        }

        private void ToNext()
        {
            _rotator.RotateTo(-90, () =>
            {
                _carusele.Next();
                _playerManager.SetCurrentTank(_carusele.CurrentIndex);
            });
        }

        private void ToPrevious()
        {
            _rotator.RotateTo(-90, () =>
            {
                _carusele.Previous();
                _playerManager.SetCurrentTank(_carusele.CurrentIndex);
            });
        }

        private void RotateObject()
        {
            _rotator.Target = _carusele.Items[_carusele.CurrentIndex];
            _rotator.StartRotate();
        }
        
        private void StopRotateObject()
        {
            _rotator.StopRotate();
        }
        
        
        private void OnStartButtonClicked()
        {
            /*var result = await _sceneLoader.LoadScene(SceneName.Game.ToString(),
                new Progress<float>(value =>
                {
                    Debug.Log($"Loading: {value:P0}");
                })
            );

            if (result)
            {
                CloseSelf();
            }*/
            _uiManager.OpenWindow(WindowType.ConnectionWindow);
        }

        private void OnCurrencvCange()
        {
            Target.CurrencyText.text =_playerManager.Data.Currency.ToString();
        }

        private void OnShopButtonClicked()
        {
            _uiManager.OpenWindow(WindowType.ShopWindow);
        }

        protected override void CloseStart()
        {
            Target.StartButton.onClick.RemoveListener(OnStartButtonClicked);
            Target.LeftButton.onClick.RemoveListener(ToPrevious);
            Target.RightButton.onClick.RemoveListener(ToNext);
            Target.RotateButtonListener.Pressed -= RotateObject;
            Target.RotateButtonListener.Released -= RotateObject;
            Target.ShopButton.onClick.RemoveListener(OnShopButtonClicked);
            Target.BankButton.onClick.RemoveListener(OnBankButton);
            // for (var i = 0; i < Target.ColorButtons.Count; i++)
            // {
            //     Target.ColorButtons[i].onClick.RemoveAllListeners();
            // }
            ShopItemColor.AddColors -= CreateColor;
            ShopItemTank.AddTank -= AddTank;
            base.CloseStart();
        }

        private void OnBankButton()
        {
            _uiManager.OpenWindow(WindowType.BankWindow);
        }
        
    }
    
}