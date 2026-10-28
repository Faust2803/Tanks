using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class ShopWindowView : BaseView
    {
       
        [SerializeField] private List<Toggle>  _shopTabToggle;
        [Space]
        [SerializeField] private List<ShopItemPanel>  _shopTabs;
        [Space]
        [SerializeField] private TextMeshProUGUI  _currencyText;
        [SerializeField] private Button  _bankButton;
        
        public List<Toggle>  ShopTabToggle => _shopTabToggle;
        public List<ShopItemPanel>  ShopTabs => _shopTabs;
        
        public Button  BankButton => _bankButton;
        public TextMeshProUGUI  CurrencyText => _currencyText;
        protected override BaseMediator CreateMediator()
        {
            _mediator = new ShopWindowMediator();
            return _mediator;
        }
    }
}