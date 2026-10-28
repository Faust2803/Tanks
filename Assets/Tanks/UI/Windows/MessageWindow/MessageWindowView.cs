using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class MessageWindowView : BaseView
    {
       
        [Space]
        [SerializeField] private TextMeshProUGUI  _titleText;
        [SerializeField] private TextMeshProUGUI  _descriptionText;
        [Space]
        [SerializeField] private Button  _okButton;
        [SerializeField] private Button  _cancelButton;
        [SerializeField] private TextMeshProUGUI  _okButtonText;
        [SerializeField] private TextMeshProUGUI  _cancelButtonText;
        
        public Button  OkButton => _okButton;
        public Button CancelButton => _cancelButton;
        public TextMeshProUGUI  TitleText => _titleText;
        public TextMeshProUGUI  DescriptionText => _descriptionText;
        
        public TextMeshProUGUI  OkButtonText => _okButtonText;
        public TextMeshProUGUI  CancelButtonText => _cancelButtonText;
        protected override BaseMediator CreateMediator()
        {
            _mediator = new MessageWindowMediator();

            _okButtonText.text = "OK";
            _cancelButtonText.text = "Cancel";

            return _mediator;
            
            
        }
    }
}