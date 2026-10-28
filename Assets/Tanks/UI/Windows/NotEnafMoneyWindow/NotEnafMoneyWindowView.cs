using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class NotEnafMoneyWindowView : BaseView
    {
       
        [Space]
        [SerializeField] private TextMeshProUGUI  _descriptionText;
        [SerializeField] private Button  _bankButton;
        [SerializeField] private Button  _cancelButton;
        
        public Button  BankButton => _bankButton;
        public Button CancelButton => _cancelButton;
        public TextMeshProUGUI  DescriptionText => _descriptionText;
        protected override BaseMediator CreateMediator()
        {
            _mediator = new NotEnafMoneyWindowMediator();
            return _mediator;
        }
    }
}