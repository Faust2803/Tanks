using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class BankWindowView : BaseView
    {
       
        
        [Space]
        [SerializeField] private TextMeshProUGUI  _currencyText;
        
        public TextMeshProUGUI  CurrencyText => _currencyText;
        protected override BaseMediator CreateMediator()
        {
            _mediator = new BankWindowMediator();
            return _mediator;
        }
    }
}