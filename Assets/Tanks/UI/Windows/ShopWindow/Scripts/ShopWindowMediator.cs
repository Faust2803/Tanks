using Tanks.Common.Scripts.Player;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    public class ShopWindowMediator :BaseWindowMediator <ShopWindowView, UIData>
    {
        
        private PlayerManager _playerManager;
        private Camera _camera;
        [Inject]
        private void Construct(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        
        private void OnCurrencvCange()
        {
            Target.CurrencyText.text =_playerManager.Data.Currency.ToString();
        }
        
        private void OnBankButton()
        {
            _uiManager.OpenWindow(WindowType.BankWindow);
            CloseSelf();
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            
            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                Target.ShopTabToggle[i].onValueChanged.AddListener(OnToggleClicked);
            }
            Target.ShopTabToggle[0].isOn = true;
            OnToggleClicked(true);
            Target.BankButton.onClick.AddListener(OnBankButton);
            OnCurrencvCange();
        }

        private void OnToggleClicked(bool isOn)
        {
            if (isOn)
            {
                for (var i = 0; i < Target.ShopTabToggle.Count; i++)
                {
                    if (Target.ShopTabToggle[i].isOn)
                    {
                        Target.ShopTabs[i].CreateObjects();
                    }
                    else
                    {
                        Target.ShopTabs[i].gameObject.SetActive(false);
                    }
                    
                    
                }
            }
        }
        
        protected override void CloseStart()
        { 
            Target.BankButton.onClick.RemoveListener(OnBankButton);
            for (var i = 0; i < Target.ShopTabToggle.Count; i++)
            {
                Target.ShopTabToggle[i].onValueChanged.RemoveListener(OnToggleClicked);
            }
            base.CloseStart();
        }
    }
}